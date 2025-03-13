using System;
using UnityEngine;
using UnityEngine.Events;

public class OnetimeTrigger : MonoBehaviour
{
    public event Action OnTriggered;

    public UnityEvent OnTriggeredEvent;
    
    private bool _triggered = false;

    public bool isActive = true;

    public void SetIsActive(bool newValue)
    {
        isActive = newValue;
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (!isActive)
            return;

        if (_triggered)
            return;
        _triggered = true;
        OnTriggered?.Invoke();
        OnTriggeredEvent?.Invoke();
    }
}