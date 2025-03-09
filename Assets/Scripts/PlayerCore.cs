using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCore : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private HealthComponent _healthComponent;
    private HealthboxComponent _healthboxComponent;


    [SerializeField] private InputActionReference unlockAction;
    [SerializeField] private InputActionReference lookAction;

    private void Start()
    {
        _playerMovement = GetComponentInChildren<PlayerMovement>();

        _healthComponent = GetComponentInChildren<HealthComponent>();

        _healthboxComponent = GetComponentInChildren<HealthboxComponent>();
        _healthboxComponent.OnHit += damage =>
        {
            _healthComponent.ChangeHealth(-damage);
            HUD.Instance.UpdateHealth(_healthComponent.CurrentHealth, _healthComponent.MaxHealth);
        };

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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


    private void OnEnable()
    {
        if (unlockAction) unlockAction.action.performed += OnUnlockPerformed;
        unlockAction.action.Enable();
    }

    private void OnDisable()
    {
        if (unlockAction) unlockAction.action.performed -= OnUnlockPerformed;
        unlockAction.action.Enable();
    }
}