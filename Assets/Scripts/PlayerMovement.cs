using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    #region Inputs

    [SerializeField] public InputActionReference moveAction;
    [SerializeField] public InputActionReference jumpAction;
    [SerializeField] public InputActionReference sprintAction;
    [SerializeField] public InputActionReference crouchAction;

    #endregion

    [SerializeField] private Transform playerInputSpace = default;


    [SerializeField, Range(0f, 100f)] private float walkMaxSpeed = 7f, sprintMaxSpeed = 10f;

    [SerializeField, Range(0f, 100f)] float
        maxAcceleration = 10f,
        maxAirAcceleration = 1f;

    [SerializeField, Range(0f, 10f)] private float jumpHeight = 2f;

    [SerializeField, Range(0, 5)] private int maxAirJumps = 0;

    [SerializeField, Range(0, 90)] private float maxGroundAngle = 25f, maxStairsAngle = 50f;

    [SerializeField, Range(0f, 100f)] private float maxSnapSpeed = 100f;

    [SerializeField, Min(0f)] private float probeDistance = 1f;

    [SerializeField] private LayerMask probeMask = -1, stairsMask = -1;

    [SerializeField] private CapsuleCollider headCollider;
    [SerializeField] private Transform head;

    private Rigidbody _body, _connectedBody, _previousConnectedBody;

    private Vector2 _playerInput;

    private Vector3 _velocity, _connectionVelocity;

    private Vector3 _connectionWorldPosition, _connectionLocalPosition;

    private Vector3 _rightAxis, _forwardAxis;
    private readonly Vector3 _upAxis = Vector3.up;

    private bool _desiredJump;

    private Vector3 _contactNormal, _steepNormal;

    private int _groundContactCount, _steepContactCount;

    private bool OnGround => _groundContactCount > 0;

    private bool OnSteep => _steepContactCount > 0;

    private int _jumpPhase;

    private float _minGroundDotProduct, _minStairsDotProduct;

    private int _stepsSinceLastGrounded, _stepsSinceLastJump;

    [SerializeField] private Vector3 gravity = Vector3.down * 9.81f * 3f;

    private Vector2 _lastMoveInput;

    private bool _crouching = false;
    private bool _tryingToUncrouch = false;

    private Tween _crouchTween;

    private Vector3 _defaultHeadLocalPosition;
    private bool _ceilingAbove;

    private void OnValidate()
    {
        _minGroundDotProduct = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);
        _minStairsDotProduct = Mathf.Cos(maxStairsAngle * Mathf.Deg2Rad);
    }

    private void Awake()
    {
        _body = GetComponent<Rigidbody>();
        _body.useGravity = false;
        OnValidate();

        _defaultHeadLocalPosition = head.localPosition;

        jumpAction.action.performed += context => { _desiredJump = true; };
        sprintAction.action.started += context => { PlayerCamera.Instance.LoadSprintFOV(); };
        sprintAction.action.canceled += context => { PlayerCamera.Instance.LoadDefaultFOV(); };
        crouchAction.action.started += context =>
        {
            if (_crouching)
                return;
            // turn off head collider
            headCollider.enabled = false;

            if (_crouchTween != null)
                _crouchTween.Kill();
            Vector3 targetPos = _defaultHeadLocalPosition;
            targetPos.y /= 2f;
            _crouchTween = DOTween.To(() => head.localPosition, x => head.localPosition = x, targetPos, 0.2f);

            _crouching = true;
        };

        crouchAction.action.canceled += context =>
        {
            if (!_crouching)
                return;
            _tryingToUncrouch = true;
        };
    }

    private void Update()
    {
        _ceilingAbove = Physics.SphereCast(PlayerCore.Instance.PlayerPosition, 1.0f, Vector3.up,
            out RaycastHit hit2, 2f, LayerMask.GetMask("Geometry"));
        if (_tryingToUncrouch){
            if (!_ceilingAbove){
                headCollider.enabled = true;


                if (_crouchTween != null)
                    _crouchTween.Kill();
                _crouchTween = DOTween.To(() => head.localPosition, x => head.localPosition = x,
                    _defaultHeadLocalPosition, 0.2f);

                _crouching = false;
                _tryingToUncrouch = false;
            }
        }

        _playerInput = moveAction.action.ReadValue<Vector2>();
        _playerInput = _playerInput.normalized * _playerInput.magnitude;

        if (_playerInput.magnitude > 0.25f)
            _lastMoveInput = _playerInput.normalized;

        if (playerInputSpace){
            _rightAxis = ProjectDirectionOnPlane(playerInputSpace.right, _upAxis);
            _forwardAxis =
                ProjectDirectionOnPlane(playerInputSpace.forward, _upAxis);
        }
        else{
            _rightAxis = ProjectDirectionOnPlane(Vector3.right, _upAxis);
            _forwardAxis = ProjectDirectionOnPlane(Vector3.forward, _upAxis);
        }
    }

    private void FixedUpdate()
    {
        UpdateState();
        AdjustVelocity();

        if (_desiredJump){
            _desiredJump = false;
            Jump(gravity);
        }

        if (OnGround && _velocity.sqrMagnitude < 0.01f){
            _velocity +=
                _contactNormal *
                (Vector3.Dot(gravity, _contactNormal) * Time.deltaTime);
        }
        else{
            _velocity += gravity * Time.deltaTime;
        }

        _body.velocity = _velocity;
        ClearState();
    }

    private void ClearState()
    {
        _groundContactCount = _steepContactCount = 0;
        _contactNormal = _steepNormal = Vector3.zero;
        _connectionVelocity = Vector3.zero;
        _previousConnectedBody = _connectedBody;
        _connectedBody = null;
    }

    private void UpdateState()
    {
        _stepsSinceLastGrounded += 1;
        _stepsSinceLastJump += 1;
        _velocity = _body.velocity;
        if (
            OnGround || SnapToGround() || CheckSteepContacts()
        ){
            _stepsSinceLastGrounded = 0;
            if (_stepsSinceLastJump > 1){
                _jumpPhase = 0;
            }

            if (_groundContactCount > 1){
                _contactNormal.Normalize();
            }
        }
        else{
            _contactNormal = _upAxis;
        }

        if (_connectedBody){
            if (_connectedBody.isKinematic || _connectedBody.mass >= _body.mass){
                UpdateConnectionState();
            }
        }
    }

    private void UpdateConnectionState()
    {
        if (_connectedBody == _previousConnectedBody){
            Vector3 connectionMovement =
                _connectedBody.transform.TransformPoint(_connectionLocalPosition) -
                _connectionWorldPosition;
            _connectionVelocity = connectionMovement / Time.deltaTime;
        }

        _connectionWorldPosition = _body.position;
        _connectionLocalPosition = _connectedBody.transform.InverseTransformPoint(
            _connectionWorldPosition
        );
    }

    private bool SnapToGround()
    {
        if (_stepsSinceLastGrounded > 1 || _stepsSinceLastJump <= 2){
            return false;
        }

        float speed = _velocity.magnitude;
        if (speed > maxSnapSpeed){
            return false;
        }

        if (!Physics.Raycast(
                _body.position, -_upAxis, out RaycastHit hit,
                probeDistance, probeMask
            )){
            return false;
        }

        float upDot = Vector3.Dot(_upAxis, hit.normal);
        if (upDot < GetMinDot(hit.collider.gameObject.layer)){
            return false;
        }

        _groundContactCount = 1;
        _contactNormal = hit.normal;
        float dot = Vector3.Dot(_velocity, hit.normal);
        if (dot > 0f){
            _velocity = (_velocity - hit.normal * dot).normalized * speed;
        }

        _connectedBody = hit.rigidbody;
        return true;
    }

    private bool CheckSteepContacts()
    {
        if (_steepContactCount > 1){
            _steepNormal.Normalize();
            float upDot = Vector3.Dot(_upAxis, _steepNormal);
            if (upDot >= _minGroundDotProduct){
                _steepContactCount = 0;
                _groundContactCount = 1;
                _contactNormal = _steepNormal;
                return true;
            }
        }

        return false;
    }

    private void AdjustVelocity()
    {
        float acceleration, speed;
        Vector3 xAxis, zAxis;
        acceleration = OnGround ? maxAcceleration : maxAirAcceleration;

        speed = sprintAction.action.ReadValue<float>() > 0.5f ? sprintMaxSpeed : walkMaxSpeed;
        xAxis = _rightAxis;
        zAxis = _forwardAxis;

        xAxis = ProjectDirectionOnPlane(xAxis, _contactNormal);
        zAxis = ProjectDirectionOnPlane(zAxis, _contactNormal);

        Vector3 relativeVelocity = _velocity - _connectionVelocity;

        Vector3 adjustment = Vector3.zero;

        adjustment.x =
            _playerInput.x * speed - Vector3.Dot(relativeVelocity, xAxis);
        adjustment.z =
            _playerInput.y * speed - Vector3.Dot(relativeVelocity, zAxis);


        adjustment =
            Vector3.ClampMagnitude(adjustment, acceleration * Time.deltaTime);

        _velocity += xAxis * adjustment.x + zAxis * adjustment.z;
    }

    private void Jump(Vector3 gravity)
    {
        Vector3 jumpDirection = Vector3.up;
        if (OnGround){
            // jumpDirection = _contactNormal;
        }
        else if (OnSteep){
            // jumpDirection = _steepNormal;
            _jumpPhase = 0;
        }
        else if (maxAirJumps > 0 && _jumpPhase <= maxAirJumps){
            if (_jumpPhase == 0){
                _jumpPhase = 1;
            }

            // jumpDirection = _contactNormal;
        }
        else{
            return;
        }

        if (_crouching && _ceilingAbove)
            return;

        _stepsSinceLastJump = 0;
        _jumpPhase += 1;
        float jumpSpeed = Mathf.Sqrt(2f * gravity.magnitude * jumpHeight);
        jumpDirection = (jumpDirection + _upAxis).normalized;
        float alignedSpeed = Vector3.Dot(_velocity, jumpDirection);
        if (alignedSpeed > 0f){
            jumpSpeed = Mathf.Max(jumpSpeed - alignedSpeed, 0f);
        }

        jumpDirection = Vector3.up;
        _velocity += jumpDirection * jumpSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        EvaluateCollision(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        EvaluateCollision(collision);
    }

    private void EvaluateCollision(Collision collision)
    {
        int layer = collision.gameObject.layer;
        float minDot = GetMinDot(layer);
        for (int i = 0; i < collision.contactCount; i++){
            Vector3 normal = collision.GetContact(i).normal;
            float upDot = Vector3.Dot(_upAxis, normal);
            if (upDot >= minDot){
                _groundContactCount += 1;
                _contactNormal += normal;
                _connectedBody = collision.rigidbody;
            }
            else{
                if (upDot > -0.01f){
                    _steepContactCount += 1;
                    _steepNormal += normal;
                    if (_groundContactCount == 0){
                        _connectedBody = collision.rigidbody;
                    }
                }
            }
        }
    }

    Vector3 ProjectDirectionOnPlane(Vector3 direction, Vector3 normal)
    {
        return (direction - normal * Vector3.Dot(direction, normal)).normalized;
    }

    float GetMinDot(int layer)
    {
        return (stairsMask & (1 << layer)) == 0 ? _minGroundDotProduct : _minStairsDotProduct;
    }


    private void OnEnable()
    {
        moveAction.action.Enable();
        jumpAction.action.Enable();
        sprintAction.action.Enable();
        crouchAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        jumpAction.action.Disable();
        sprintAction.action.Disable();
        crouchAction.action.Disable();
    }
}