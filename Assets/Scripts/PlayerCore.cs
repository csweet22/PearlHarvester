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
    
    [SerializeField] private InputActionReference lookAction;

    [SerializeField] private InputActionReference pauseAction;
    [SerializeField] private GameObject pauseMenu;

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

    public void AddAxe()
    {
        _playerAxeManager.ChangeAxeCount(1);
    }


    private void OnEnable()
    {
        pauseAction.action.Enable();
        pauseAction.action.performed += OnActionOnperformed;
    }

    private void OnActionOnperformed(InputAction.CallbackContext context)
    {
        if (!GameManager.Instance.paused){
            MainCanvas.Instance.OpenMenu(pauseMenu, Vector3.zero, 0.0f);
        }
        else{
            MainCanvas.Instance.CloseMenu();
        }
    }

    private void OnDisable()
    {
        pauseAction.action.Disable();
        pauseAction.action.performed -= OnActionOnperformed;
    }
}