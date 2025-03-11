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

    public override void Open()
    {
        base.Open();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public override void Close()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        startGameButton.onClick.AddListener(OnStartGameButtonClicked);
        settingsButton.onClick.AddListener(OnSettingsClicked);
    }

    private void OnStartGameButtonClicked()
    {
        MainCanvas.Instance.CloseAllMenus();
        SceneManager.LoadScene(startGameSceneName);
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