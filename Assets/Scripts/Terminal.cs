using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Terminal : MonoBehaviour
{
    [SerializeField] private GameObject terminalMenu;
    
    [SerializeField] private InputActionReference pauseAction;
    
    private void OnEnable()
    {
        pauseAction.action.Enable();
        pauseAction.action.performed += OnPausePerformed;
    }

    private void OnPausePerformed(InputAction.CallbackContext obj)
    {
        if (!GameManager.Instance.inTerminal)
            return;
        MainCanvas.Instance.CloseMenu();
    }

    private void OnDisable()
    {
        pauseAction.action.Disable();
    }

    public void OpenTerminal()
    {
        MainCanvas.Instance.OpenMenu(terminalMenu, Vector3.up);
    }
}
