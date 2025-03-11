using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : ACMenu
{
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button settingsButton;
    
    [SerializeField] private GameObject settingsMenu;

    [SerializeField] private string startGameSceneName;
    
    // Start is called before the first frame update
    void OnEnable()
    {
        startGameButton.onClick.AddListener(OnStartGameButtonClicked);
        settingsButton.onClick.AddListener(OnSettingsClicked);
    }

    private void OnStartGameButtonClicked()
    {
        SceneManager.LoadScene(startGameSceneName);
        MainCanvas.Instance.CloseMenu(0.0f);
    }

    private void OnSettingsClicked()
    {
        MainCanvas.Instance.OpenMenu(settingsMenu, Vector3.right);
    }

    private void OnDisable()
    {
        startGameButton.onClick.RemoveAllListeners();
        settingsButton.onClick.RemoveAllListeners();
    }
}