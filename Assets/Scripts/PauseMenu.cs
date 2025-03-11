using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Scripts.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PauseMenu : ACMenu
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button mainMenuButton;

    [SerializeField] private GameObject settingsMenu;

    [SerializeField] private string mainMenuSceneName = "Game";

    public override void Open()
    {
        base.Open();
        Pause();
    }

    public override void Close()
    {
        base.Close();
        Unpause();
    }

    private void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GameManager.Instance.paused = true;
        Time.timeScale = 0f;
    }

    private void Unpause()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1.0f;
        GameManager.Instance.paused = false;
    }

    private void OnEnable()
    {
        resumeButton.onClick.AddListener(Resume);
        settingsButton.onClick.AddListener(Settings);
        mainMenuButton.onClick.AddListener(MainMenu);
    }

    private void MainMenu()
    {
        MainCanvas.Instance.LoadMainMenu();
        SceneManager.LoadScene(mainMenuSceneName);
    }

    private void Settings()
    {
        MainCanvas.Instance.OpenMenu(settingsMenu, Vector3.right);
    }

    private void Resume()
    {
        MainCanvas.Instance.CloseMenu(0.0f);
    }

    private void OnDisable()
    {
        resumeButton.onClick.RemoveAllListeners();
        settingsButton.onClick.RemoveAllListeners();
        mainMenuButton.onClick.RemoveAllListeners();
    }
}