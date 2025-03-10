using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCore : Singleton<PlayerCore>
{
    private PlayerMovement _playerMovement;
    private HealthComponent _healthComponent;
    private HealthboxComponent _healthboxComponent;
    private PlayerAxeManager _playerAxeManager;

    public Vector3 PlayerPosition => _playerMovement.transform.position;

    [SerializeField] private InputActionReference unlockAction;
    [SerializeField] private InputActionReference lookAction;

    private void Start()
    {
        _playerMovement = GetComponentInChildren<PlayerMovement>();

        _healthComponent = GetComponentInChildren<HealthComponent>();

        _healthboxComponent = GetComponentInChildren<HealthboxComponent>();
        _healthboxComponent.OnHit += delta =>
        {
            _healthComponent.ChangeHealth(delta);
            HUD.Instance.UpdateHealth(_healthComponent.CurrentHealth, _healthComponent.MaxHealth);
        };

        _playerAxeManager = GetComponentInChildren<PlayerAxeManager>();
        
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

    public void AddAxe()
    {
        _playerAxeManager.ChangeAxeCount(1);
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