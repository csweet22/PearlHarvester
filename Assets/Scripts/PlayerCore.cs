using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCore : MonoBehaviour
{
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference jumpAction;
    [SerializeField] private InputActionReference unlockAction;
    [SerializeField] private InputActionReference lookAction;


    [SerializeField] private float speed = 10f;

    [SerializeField] private float jumpForce = 7f;
    public float groundCheckDistance = 1.0f;

    private Rigidbody _rb;
    private Camera _mainCamera;

    private void OnEnable()
    {
        if (moveAction) moveAction.action.performed += OnMovePerformed;
        if (jumpAction) jumpAction.action.performed += OnJumpPerformed;
        if (unlockAction) unlockAction.action.performed += OnUnlockPerformed;

        moveAction.action.Enable();
        jumpAction.action.Enable();
        unlockAction.action.Enable();
    }

    private void OnDisable()
    {
        if (moveAction) moveAction.action.performed -= OnMovePerformed;
        if (jumpAction) jumpAction.action.performed -= OnJumpPerformed;
        if (unlockAction) unlockAction.action.performed -= OnUnlockPerformed;

        moveAction.action.Disable();
        jumpAction.action.Disable();
        unlockAction.action.Enable();
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _mainCamera = Camera.main;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        ResolveMovement();
    }

    private void ResolveMovement()
    {
        Vector2 moveValue = moveAction.action.ReadValue<Vector2>();

        Vector3 cameraForward = _mainCamera.transform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();

        Vector3 cameraRight = _mainCamera.transform.right;
        cameraRight.y = 0f;
        cameraRight.Normalize();

        Vector3 moveDirection = cameraForward * moveValue.y + cameraRight * moveValue.x;

        Vector3 targetVelocity = moveDirection * speed;
        Vector3 currentVelocity = _rb.velocity;

        Vector3 deltaVelocity = targetVelocity - currentVelocity;
        deltaVelocity.y = 0f;
        _rb.AddForce(deltaVelocity, ForceMode.VelocityChange);
    }

    private void OnJumpPerformed(InputAction.CallbackContext obj)
    {
        if (IsGrounded())
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
    }

    private void OnMovePerformed(InputAction.CallbackContext obj)
    {
    }

    private void OnUnlockPerformed(InputAction.CallbackContext obj)
    {
        if (Cursor.lockState == CursorLockMode.Locked){
            Cursor.lockState = CursorLockMode.None;
            lookAction.action.Disable();
            Cursor.visible = true;
        }
        else{
            Cursor.lockState = CursorLockMode.Locked;
            lookAction.action.Enable();
            Cursor.visible = false;
        }
    }


    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position + Vector3.up, Vector3.down, groundCheckDistance);
    }
}