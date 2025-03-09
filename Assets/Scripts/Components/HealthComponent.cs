using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] protected int maxHealth = 10;
    public int MaxHealth => maxHealth;

    [SerializeField] protected int startingHealth = 1;
    private readonly Trackable<int> _currentHealth = new(0);
    public int Health => _currentHealth.Value;

    private bool _isInvincible = false;
    public bool IsInvincible => _isInvincible;

    
    public event Action<int> OnGainHealth;
    public event Action<int> OnLoseHealth;
    public event Action OnHealthEmpty;
    
    protected void Start()
    {
        _currentHealth.Value = startingHealth;

        _currentHealth.OnValueSet += (oldValue, newValue) =>
        {
            if (newValue == 0){
                OnHealthEmpty?.Invoke();
            }
        };
    }
    
    public void ChangeHealth(int delta = 1)
    {
        _currentHealth.Value += delta;

        if (delta > 0)
            OnGainHealth?.Invoke(delta);
        else if (delta < 0)
            OnLoseHealth?.Invoke(delta);
    }

    public void SetHealth(int newHealth)
    {
        _currentHealth.Value = newHealth;
    }

    public void SetInvincible(bool isInvincible)
    {
        _isInvincible = isInvincible;
    }
}