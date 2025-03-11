using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMouseControl : MonoBehaviour
{
    [SerializeField] private InputActionReference lookAction;


    [SerializeField] public Transform head;

    [SerializeField] private float minYAngle = -80f;
    [SerializeField] private float maxYAngle = 80f;

    private float _currentXRotation = 0f;
    private float _currentYRotation = 0f;

    private void Update()
    {
        if (GameManager.Instance.paused)
            return;

        float sensitivity = Settings.Instance.sensitivity;
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        float mouseX = mouseDelta.x * sensitivity;
        float mouseY = mouseDelta.y * sensitivity;

        _currentYRotation -= mouseY;
        _currentYRotation = Mathf.Clamp(_currentYRotation, minYAngle, maxYAngle);

        _currentXRotation += mouseX;

        head.localRotation = Quaternion.Euler(_currentYRotation, _currentXRotation, 0f);
    }

    private void OnEnable()
    {
        lookAction.action.Enable();
    }

    private void OnDisable()
    {
        lookAction.action.Disable();
    }
}