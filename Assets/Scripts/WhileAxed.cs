using System;
using System.Collections;
using System.Collections.Generic;
using Content.Scripts.Components;
using UnityEngine;
using UnityEngine.Events;

public class WhileAxed : MonoBehaviour
{
    private InteractableComponent _interactable;

    private bool _isBeingUsed = false;

    private TimerComponent _timer;

    [SerializeField] private Collider physicsCollider;

    public UnityEvent onAxeHit;
    public UnityEvent onAxeRemoved;

    private void Start()
    {
        _interactable = GetComponentInChildren<InteractableComponent>();
        _interactable.StartInteractAction += OnInteractStarted;
        _interactable.EndInteractAction += component => { TurnOff(); };

        _timer = GetComponentInChildren<TimerComponent>();
        if (_timer)
            _timer.OnTimeoutAction += () => { TurnOff(); };
    }

    private void OnInteractStarted(InteractorComponent obj)
    {
        if (CompareTag("organ")){
            GameManager.Instance.PlayGroan();
        }

        if (obj.tag == "AxeProjectile"){
            obj.GetComponentInParent<AxeProjectile>().Connect(physicsCollider, transform);
            TurnOn();
        }
        else{
            if (_isBeingUsed)
                return;
            TurnOn();
            _timer?.StartTimer(0.0f);
        }
    }

    public void TurnOn()
    {
        Debug.Log($"{gameObject.name}: Turned on.");
        _timer?.StopTimer();
        _isBeingUsed = true;
        onAxeHit?.Invoke();
    }


    public void TurnOff()
    {
        Debug.Log($"{gameObject.name}: Turned off.");
        _isBeingUsed = false;
        onAxeRemoved?.Invoke();
    }
}