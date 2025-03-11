using System;
using System.Collections;
using System.Collections.Generic;
using Content.Scripts.Components;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class AxeProjectile : ProjectileComponent
{
    [SerializeField] private InputActionReference recallAction;
    [SerializeField] private Collider physicsCollider;
    [SerializeField] private HealthChangeBoxComponent healthChangeBox;

    private InteractorComponent _interactor;
    private Collider _connectedCollider = null;
    private bool _isConnected = false;

    Vector3 localConnectPosition = Vector3.zero;
    Vector3 localConnectRotation = Vector3.zero;

    [SerializeField] private float rotationSpeed = 200f;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddTorque(rb.transform.right * rotationSpeed);
        healthChangeBox.OnHit += targetHealthbox =>
        {
            healthChangeBox.enabled = false;
        };

        _interactor = GetComponentInChildren<InteractorComponent>();
        _interactor.StartInteractAction += () =>
        {
            _interactor.canInteract = false;
        };
    }

    private void OnEnable()
    {
        recallAction.action.Enable();

        recallAction.action.performed += OnRecallPerformed;
    }

    private void OnRecallPerformed(InputAction.CallbackContext obj)
    {
        _isConnected = false;
        
        // Turn of physics collider
        physicsCollider.enabled = false;

        // Lerp towards player
        Tween lerp = DOTween.To(() => gameObject.transform.position, x => gameObject.transform.position = x,
            PlayerCore.Instance.PlayerPosition, 0.2f);
        // Once at the player, increment axes & despawn
        lerp.onComplete += () =>
        {
            PlayerCore.Instance.AddAxe();

            recallAction.action.performed -= OnRecallPerformed;
            Destroy(gameObject);
        };
    }

    private void OnDisable()
    {
        recallAction.action.Disable();

        recallAction.action.performed -= OnRecallPerformed;
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        if (!_connectedCollider && !_isConnected){
            Connect(collision);
        }

        GetComponentInChildren<InteractorComponent>().DeactivateInteractable();
    }

    private void Update()
    {
        if (!_connectedCollider && _isConnected){
            Disconnect();
        }

        if (_isConnected){
            transform.localPosition = localConnectPosition;
            transform.localEulerAngles = localConnectRotation;
        }
    }

    private void Connect(Collision collision)
    {
        _isConnected = true;
        _connectedCollider = collision.collider;

        Rigidbody connectedRb = _connectedCollider.gameObject.GetComponent<Rigidbody>();
        if (connectedRb)
            transform.parent = connectedRb.transform;
        else{
            transform.parent = collision.transform;
        }
        
        localConnectPosition = transform.localPosition;
        localConnectRotation = transform.localEulerAngles;

        rb.isKinematic = true;
        healthChangeBox.enabled = false;
    }

    private void Disconnect()
    {
        _isConnected = false;
        _connectedCollider = null;
        transform.parent = null;
        rb.isKinematic = false;
        healthChangeBox.enabled = true;
        rb.velocity = Vector3.zero;
        _interactor.canInteract = true;
    }
}