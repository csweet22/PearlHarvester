using System;
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

    private void Start()
    {
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