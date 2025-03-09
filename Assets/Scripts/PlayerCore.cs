using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCore : MonoBehaviour
{
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference jumpAction;


    [SerializeField] private float speed = 10f;

    private Rigidbody _rb;
    private Camera _mainCamera;

    private void OnEnable()
    {
        if (moveAction) moveAction.action.performed += OnMovePerformed;
        if (jumpAction) jumpAction.action.performed += OnJumpPerformed;

        moveAction.action.Enable();
        jumpAction.action.Enable();
    }

    private void OnDisable()
    {
        if (moveAction) moveAction.action.performed -= OnMovePerformed;
        if (jumpAction) jumpAction.action.performed -= OnJumpPerformed;

        moveAction.action.Disable();
        jumpAction.action.Disable();
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _mainCamera = Camera.main;
    }

    private void FixedUpdate()
    {
        ResolveMovement();
    }

    private void ResolveMovement()
    {
        Vector2 moveValue = moveAction.action.ReadValue<Vector2>();
        
        Vector3 cameraForward = _mainCamera.transform.forward ;
        cameraForward.y = 0f;
        cameraForward.Normalize();

        Vector3 cameraRight = _mainCamera.transform.right;
        cameraRight.y = 0f;
        cameraRight.Normalize();
        
        Vector3 moveDirection = cameraForward * moveValue.y + cameraRight * moveValue.x;
        
        Vector3 targetVelocity = moveDirection * speed;
        Vector3 currentVelocity = _rb.velocity;

        Vector3 deltaVelocity = targetVelocity - currentVelocity;
        Debug.Log(deltaVelocity);
        _rb.AddForce(deltaVelocity, ForceMode.VelocityChange);
    }

    private void OnJumpPerformed(InputAction.CallbackContext obj)
    {
    }

    private void OnMovePerformed(InputAction.CallbackContext obj)
    {
    }
}