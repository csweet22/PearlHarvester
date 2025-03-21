using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Scripts.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HUD : Singleton<HUD>
{
    [SerializeField] private ProgressBar healthBar;
    [SerializeField] private Texture reticleActiveTexture;
    [SerializeField] private RawImage reticle;

    [SerializeField] private TextMeshProUGUI quota;
    [SerializeField] private TextMeshProUGUI total;

    [SerializeField] private Image tint;

    [SerializeField] private TextMeshProUGUI timer;

    public float reticleActiveScale = 10f;

    private Tween _tintTween;


    private bool showTimer;
    private void Start()
    {
        ShowTimer(Settings.Instance.showTimer);
        UpdateQuotaAndTotal();
        GameManager.Instance.PearlCount.OnValueChanged += (i, i1) => { UpdateQuotaAndTotal(); };
    }

    public void UpdateQuotaAndTotal()
    {
        if (total)
            total.text = $"TOTAL: {GameManager.Instance.PearlCount.Value} / {GameManager.Instance.totalPearlCount}";
        if (quota)
            quota.text = $"QUOTA: {GameManager.Instance.PearlCount.Value} / {GameManager.Instance.quota}";
    }

    public Color GetTint()
    {
        return tint.color;
    }

    public void SetTint(Color tintColor)
    {
        if (_tintTween != null)
            _tintTween.Kill();
        tint.color = tintColor;
    }

    public void TweenTint(Color tintColor, float duration = 0.2f)
    {
        if (_tintTween != null)
            _tintTween.Kill();
        _tintTween = DOTween.To(() => tint.color, x => tint.color = x, tintColor, duration);
    }

    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        healthBar.max.Value = maxHealth;
        healthBar.Progress.Value = currentHealth;

        // Debug.Log($"{currentHealth} / {maxHealth}");
    }

    public void SetReticleInteractable()
    {
        reticle.color = new Color(0, 1f, 0, 1f);
        reticle.texture = reticleActiveTexture;
        reticle.rectTransform.localScale = new Vector3(reticleActiveScale, reticleActiveScale, reticleActiveScale);
    }

    public void SetReticleDefault()
    {
        reticle.color = new Color(1f, 1f, 1f, 1f);
        reticle.texture = null;
        reticle.rectTransform.localScale = new Vector3(1, 1, 1);
    }

    public void ShowTimer(bool instanceShowTimer)
    {
        showTimer = instanceShowTimer;
    }

    private void Update()
    {
        if (!showTimer){
            timer.text = "";
            return;
        }
        
        int minutes = Mathf.FloorToInt(GameManager.Instance._runTime / 60);
        float seconds = GameManager.Instance._runTime % 60;
        int milliseconds = Mathf.FloorToInt((seconds - Mathf.Floor(seconds)) * 1000);
        
        timer.text = $"{minutes:D2}:{Mathf.FloorToInt(seconds):D2}.{milliseconds:D3}";
    }
}