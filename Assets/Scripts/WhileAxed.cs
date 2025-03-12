using System;
using System.Collections;
using System.Collections.Generic;
using Content.Scripts.Components;
using UnityEngine;

public class WhileAxed : MonoBehaviour
{
    private InteractableComponent _interactable;

    private bool _isBeingUsed = false;

    private TimerComponent _timer;

    [SerializeField] private Collider physicsCollider;
    
    private void Start()
    {
        _interactable = GetComponentInChildren<InteractableComponent>();
        _interactable.StartInteractAction += OnInteractStarted;
        _interactable.EndInteractAction += component => { TurnOff(); };

        _timer = GetComponentInChildren<TimerComponent>();
        _timer.OnTimeoutAction += OnTimeOut;
    }

    private void OnTimeOut()
    {
        TurnOff();
    }

    private void OnInteractStarted(InteractorComponent obj)
    {
        if (obj.tag == "AxeProjectile"){
            obj.GetComponentInParent<AxeProjectile>().Connect(physicsCollider, transform);
            TurnOn();
        }
        else{
            if (_isBeingUsed) // Move this to TurnOn if you want to be able to keep hitting it to keep it on
                return;
            TurnOn();
            _timer.StartTimer(0.0f);
        }
    }

    public void TurnOn()
    {
        _isBeingUsed = true;
        Debug.Log("Button On");
    }


    public void TurnOff()
    {
        _isBeingUsed = false;
        Debug.Log("Button Off");
    }
}