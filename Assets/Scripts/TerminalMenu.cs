using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Scripts.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class TerminalMenu : ACMenu
{
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button backButton;

    [SerializeField] private TextMeshProUGUI quotaText;
    [SerializeField] private TextMeshProUGUI upgradeText;

    public override void Open()
    {
        base.Open();
        GameManager.Instance.Pause();
        UpdateQuotaText();
        GameManager.Instance.PearlCount.OnValueChanged += OnPearlValueChanged;
    }

    private void OnPearlValueChanged(int oldValue, int newValue)
    {
        UpdateQuotaText();
        UpdateUpgradeText();
    }

    private void UpdateQuotaText()
    {
        Pearl[] allPearls = FindObjectsOfType<Pearl>();
        quotaText.text = $"<b>QUOTA</b>: {GameManager.Instance.PearlCount} / {allPearls.Length} TOTAL PEARLS";
    }

    private void UpdateUpgradeText()
    {
        upgradeText.text = "";
        for (int i = 0; i < GameManager.Instance.requiredPearls.Count; i++){
            int requiredCount = GameManager.Instance.requiredPearls[i];

            if (GameManager.Instance.PearlCount.Value >= requiredCount){
                ;
            }
            else{
                upgradeText.text += $"{requiredCount - GameManager.Instance.PearlCount.Value} PEARLS: UPGRADE {i + 1}";
            }
        }
    }

    public override void Close()
    {
        base.Close();
        GameManager.Instance.PearlCount.OnValueChanged -= OnPearlValueChanged;
        GameManager.Instance.Unpause();
    }

    private void OnEnable()
    {
        upgradeButton.onClick.AddListener(Upgrade);
        backButton.onClick.AddListener(Back);
    }

    private void MainMenu()
    {
        MainCanvas.Instance.LoadMainMenu();
    }

    private void Back()
    {
        MainCanvas.Instance.CloseMenu(0.0f);
    }

    private void Upgrade()
    {
    }

    private void OnDisable()
    {
        upgradeButton.onClick.RemoveAllListeners();
        backButton.onClick.RemoveAllListeners();
    }
}