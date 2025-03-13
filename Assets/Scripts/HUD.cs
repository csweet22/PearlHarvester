using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Utilities;
using UnityEngine;
using UnityEngine.UI;

public class HUD : Singleton<HUD>
{
    [SerializeField] private ProgressBar healthBar;
    [SerializeField] private RawImage reticle;
    
    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        healthBar.max.Value = maxHealth;
        healthBar.Progress.Value = currentHealth;

        Debug.Log($"{currentHealth} / {maxHealth}");
    }

    public void SetReticleInteractable()
    {
        reticle.color = new Color(0, 1f, 0, 1f);
    }

    public void SetReticleDefault()
    {
        reticle.color = new Color(1f, 1f, 1f, 1f);
    }
}