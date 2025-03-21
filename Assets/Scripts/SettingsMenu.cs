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
    [SerializeField] private Slider fovSlider;
    [SerializeField] private Button backButton;
    [SerializeField] private Toggle showTimerButton;

    private void OnEnable()
    {
        sensitivitySlider.value = Settings.Instance.sensitivity;
        sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);

        fovSlider.value = Settings.Instance.fov;
        fovSlider.onValueChanged.AddListener(OnFOVChanged);

        backButton.onClick.AddListener(OnBackClicked);

        showTimerButton.isOn = Settings.Instance.showTimer;
        showTimerButton.onValueChanged.AddListener(OnShowTimerClicked);
    }

    private void OnShowTimerClicked(bool newValue)
    {
        Settings.Instance.showTimer = newValue;
        HUD.Instance.ShowTimer(Settings.Instance.showTimer);
    }

    private void OnBackClicked()
    {
        MainCanvas.Instance.CloseMenu();
    }

    private void OnSensitivityChanged(float newValue)
    {
        Settings.Instance.sensitivity = newValue;
    }

    private void OnFOVChanged(float newValue)
    {
        Settings.Instance.fov = newValue;
        if (PlayerCamera.Instance != null){
            PlayerCamera.Instance.LoadDefaultFOV(0.0f);
        }
    }

    private void OnDisable()
    {
        sensitivitySlider.onValueChanged.RemoveAllListeners();
        fovSlider.onValueChanged.RemoveAllListeners();
        backButton.onClick.RemoveAllListeners();
    }
}