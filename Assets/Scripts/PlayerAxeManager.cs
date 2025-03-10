using System;
using Content.Scripts.Components;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerAxeManager : MonoBehaviour
{
    private int _currentAxeCount = 1;
    private int _maxAxeCount = 1;
    public bool HasAxe => _currentAxeCount > 0;

    private bool _throwQueued = false;

    [SerializeField] private InputActionReference swingAction;
    [SerializeField] private InputActionReference throwAction;
    [SerializeField] private InputActionReference recallAction;

    private Animator _animator;
    private LauncherComponent _launcher;
    private HealthChangeBoxComponent _healthChangeBox;
    private InteractorComponent _interactor;

    private void Start()
    {
        _launcher = GetComponent<LauncherComponent>();
        _animator = GetComponent<Animator>();
        _healthChangeBox = GetComponentInChildren<HealthChangeBoxComponent>();
        _interactor = GetComponentInChildren<InteractorComponent>();
        
        DeactivateDamage();
    }

    public void ActivateDamage()
    {
        _healthChangeBox.enabled = true;
        _interactor.ActivateInteractable();
    }

    public void DeactivateDamage()
    {
        _healthChangeBox.enabled = false;
        _interactor.DeactivateInteractable();
    }

    public void ChangeAxeCount(int delta)
    {
        _currentAxeCount += delta;

        if (_currentAxeCount < 0)
            _currentAxeCount = 0;

        if (_currentAxeCount > _maxAxeCount)
            _currentAxeCount = _maxAxeCount;
    }

    private void OnEnable()
    {
        swingAction.action.Enable();
        throwAction.action.Enable();
        recallAction.action.Enable();

        swingAction.action.performed += OnSwingPerformed;

        throwAction.action.performed += OnThrowPrepared;
        throwAction.action.canceled += OnThrowReleased;

        recallAction.action.started += OnRecallPerformed;
    }

    private void OnSwingPerformed(InputAction.CallbackContext obj)
    {
        Debug.Log("Swing performed");
        _animator.Play("Swing");
    }

    private void OnThrowPrepared(InputAction.CallbackContext obj)
    {
        if (HasAxe){
            _throwQueued = true;
            Debug.Log("Throw prepared");
        }
    }

    private void OnThrowReleased(InputAction.CallbackContext obj)
    {
        if (_throwQueued){
            _throwQueued = false;
            Debug.Log("Axe Thrown.");
            LaunchParameters launchParameters = new LaunchParameters(Camera.main.transform.forward, 5f);
            _launcher.Launch(launchParameters);
            _currentAxeCount -= 1;
        }
    }

    private void OnRecallPerformed(InputAction.CallbackContext obj)
    {
        Debug.Log("Recall performed");
    }


    private void OnDisable()
    {
        swingAction.action.Disable();
        throwAction.action.Disable();
        recallAction.action.Disable();

        swingAction.action.performed -= OnSwingPerformed;

        throwAction.action.performed -= OnThrowPrepared;
        throwAction.action.canceled -= OnThrowReleased;

        recallAction.action.started -= OnRecallPerformed;
    }
}