using System;
using System.Collections;
using System.Collections.Generic;
using Content.Scripts.Components;
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

    [SerializeField] private Transform head;

    private void Start()
    {
        _playerMovement = GetComponentInChildren<PlayerMovement>();

        _healthComponent = GetComponentInChildren<HealthComponent>();

        _healthComponent.OnHealthEmpty += () =>
        {
            _playerMovement.transform.position = GameManager.Instance.safeAreaSpawn.position;
            _healthComponent.SetHealth(_healthComponent.MaxHealth);
        };

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

    private void FixedUpdate()
    {
        // Check if facing interactable
        if (Physics.Raycast(head.position, head.forward, out RaycastHit hit, 100f,
                LayerMask.GetMask("Geometry", "Interaction"))){
            if (hit.rigidbody){
                InteractableComponent interactableComponent =
                    hit.rigidbody.GetComponentInChildren<InteractableComponent>();
                if (interactableComponent || hit.rigidbody.CompareTag("Door")){
                    HUD.Instance.SetReticleInteractable();
                }
                else{
                    HUD.Instance.SetReticleDefault();
                }
            }
            else{
                HUD.Instance.SetReticleDefault();
            }
        }
        else{
            HUD.Instance.SetReticleDefault();
        }
    }

    public void AddAxe()
    {
        _playerAxeManager.ChangeAxeCount(1);
    }


    private void OnEnable()
    {
        pauseAction.action.Enable();
        pauseAction.action.performed += OnPausePerformed;
    }

    private void OnPausePerformed(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.inTerminal){
            return;
        }

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
        pauseAction.action.performed -= OnPausePerformed;
    }
}