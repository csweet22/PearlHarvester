using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Scripts.Utilities;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : ACMenu
{
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private Button backButton;
    
    private void OnEnable()
    {
        sensitivitySlider.value = Settings.Instance.sensitivity;
        sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);
        backButton.onClick.AddListener(OnBackClicked);
    }

    private void OnBackClicked()
    {
        MainCanvas.Instance.CloseMenu();
    }

    private void OnSensitivityChanged(float newValue)
    {
        Settings.Instance.sensitivity = newValue;
    }

    private void OnDisable()
    {
        sensitivitySlider.onValueChanged.RemoveAllListeners();
        backButton.onClick.RemoveAllListeners();
    }
}