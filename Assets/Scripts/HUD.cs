using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HUD : Singleton<HUD>
{
    [SerializeField] private ProgressBar healthBar;
    [SerializeField] private RawImage reticle;

    [SerializeField] private TextMeshProUGUI quota;
    [SerializeField] private TextMeshProUGUI total;

    private void Start()
    {
        UpdateQuotaAndTotal();
        GameManager.Instance.PearlCount.OnValueChanged += (i, i1) => { UpdateQuotaAndTotal(); };
    }

    private void UpdateQuotaAndTotal()
    {
        if (total)
            total.text = $"TOTAL: {GameManager.Instance.PearlCount.Value} / {GameManager.Instance.totalPearlCount}";
        if (quota)
            quota.text = $"QUOTA: {GameManager.Instance.PearlCount.Value} / {GameManager.Instance.quota}";
    }

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