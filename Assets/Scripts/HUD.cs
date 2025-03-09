using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Utilities;
using UnityEngine;

public class HUD : Singleton<HUD>
{
    [SerializeField] private ProgressBar healthBar;

    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        healthBar.max.Value = maxHealth;
        healthBar.Progress.Value = currentHealth;

        Debug.Log($"{currentHealth} / {maxHealth}");
    }
}