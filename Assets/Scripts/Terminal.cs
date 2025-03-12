using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Terminal : MonoBehaviour
{
    [SerializeField] private GameObject terminalMenu;

    [SerializeField] private InputActionReference pauseAction;

    [SerializeField] private float openDelay = 0.5f;

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

    IEnumerator DelayedOpen()
    {
        yield return new WaitForSeconds(openDelay);
        MainCanvas.Instance.OpenMenu(terminalMenu, Vector3.up);
    }

    private void OnDisable()
    {
        pauseAction.action.Disable();
    }

    public void OpenTerminal()
    {
        StartCoroutine(DelayedOpen());
    }
}