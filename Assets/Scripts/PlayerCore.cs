using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCore : MonoBehaviour
{
    
    private PlayerMovement _playerMovement;
    
    [SerializeField] private InputActionReference unlockAction;
    [SerializeField] private InputActionReference lookAction;

    private void Start()
    {
        _playerMovement = GetComponentInChildren<PlayerMovement>();
        
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