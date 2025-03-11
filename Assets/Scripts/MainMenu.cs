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

    // Start is called before the first frame update
    void OnEnable()
    {
        startGameButton.onClick.AddListener(OnStartGameButtonClicked);
        settingsButton.onClick.AddListener(OnSettingsClicked);
    }

    private void OnStartGameButtonClicked()
    {
        
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