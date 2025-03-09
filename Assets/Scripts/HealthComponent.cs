using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public int Health => _health.Value;
    private readonly Trackable<int> _health = new(0);

    public event Action OnHitZeroHealth;
    public event Action<int> OnGainHealth;
    public event Action<int> OnLoseHealth;

    private void Start()
    {
        _health.OnValueSet += (oldValue, newValue) =>
        {
            if (newValue == 0){
                OnHitZeroHealth?.Invoke();
            }
        };
    }

    public void DeltaHealth(int delta)
    {
        _health.Value += delta;
        
        if (delta > 0)
            OnGainHealth?.Invoke(delta);
        else if (delta < 0)
            OnLoseHealth?.Invoke(delta);
    }
}