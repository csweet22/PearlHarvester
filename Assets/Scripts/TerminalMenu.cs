using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private TextMeshProUGUI upgradeTextHeader;

    private bool CanUpgrade()
    {
        if (GameManager.Instance.upgradesUnlocked >= GameManager.Instance.requiredPearls.Count)
            return false;

        return GameManager.Instance.PearlCount.Value >=
               GameManager.Instance.requiredPearls[GameManager.Instance.upgradesUnlocked];
    }

    public override void Open()
    {
        base.Open();
        GameManager.Instance.Pause();
        GameManager.Instance.inTerminal = true;
        UpdateQuotaText();
        UpdateUpgradeText();
        UpdateUpgradeButtonInteractable();
        GameManager.Instance.PearlCount.OnValueChanged += OnPearlValueChanged;
        if (GameManager.Instance.quotaReached){
            GameManager.Instance.EndGame();
        }
    }

    private void OnPearlValueChanged(int oldValue, int newValue)
    {
        UpdateQuotaText();
        UpdateUpgradeText();
        UpdateUpgradeButtonInteractable();
    }

    private void UpdateQuotaText()
    {
        quotaText.text =
            $"<b>QUOTA</b>: {GameManager.Instance.PearlCount.Value} / {GameManager.Instance.quota} PEARLS";

        if (GameManager.Instance.quotaReached){
            quotaText.text =
                $"YOU REACHED QUOTA.\nTHANK YOU FOR PLAYING.\nCOLLECTED {GameManager.Instance.PearlCount.Value} / {GameManager.Instance.totalPearlCount} PEARLS";
            GameManager.Instance._risingBlood.gameObject.SetActive(false);
        }
    }

    private void UpdateUpgradeText()
    {
        upgradeText.text = "";
        for (int i = 0; i < GameManager.Instance.requiredPearls.Count; i++){
            int requiredCount = GameManager.Instance.requiredPearls[i];
            if (i >= GameManager.Instance.upgradesUnlocked){
                if (i == 0){
                    upgradeText.text +=
                        $"{Mathf.Max(requiredCount - GameManager.Instance.PearlCount.Value, 0)} MORE PEARLS UNTIL <b>AXE THROW SPEED</b>\n";
                }

                if (i == 1){
                    upgradeText.text +=
                        $"{Mathf.Max(requiredCount - GameManager.Instance.PearlCount.Value, 0)} MORE PEARLS UNTIL <b>CAN BREAK BONE DOORS</b>\n";
                }
            }
        }

        if (GameManager.Instance.requiredPearls.Count == 0){
            upgradeTextHeader.text = "";
        }


        if (GameManager.Instance.quotaReached){
            upgradeTextHeader.text = "";
            upgradeText.text = $"";
        }
    }

    public override void Close()
    {
        base.Close();
        GameManager.Instance.inTerminal = false;
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

    private void UpdateUpgradeButtonInteractable()
    {
        upgradeButton.interactable = CanUpgrade();
    }

    private void Upgrade()
    {
        if (CanUpgrade()){
            GameManager.Instance.Upgrade();
        }

        UpdateUpgradeButtonInteractable();
        UpdateUpgradeText();
    }

    private void OnDisable()
    {
        upgradeButton.onClick.RemoveAllListeners();
        backButton.onClick.RemoveAllListeners();
    }
}