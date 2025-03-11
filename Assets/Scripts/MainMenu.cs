using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenu : ACMenu
{
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button settingsButton;

    [SerializeField] private Scene gameScene;

    // Start is called before the first frame update
    void OnEnable()
    {
        startGameButton.clicked += OnStartGameButtonClicked;
    }

    private void OnStartGameButtonClicked()
    {
        if (gameScene != null) SceneManager.LoadScene(gameScene.buildIndex);
    }

    private void OnDisable()
    {
        startGameButton.clicked -= OnStartGameButtonClicked;
    }
}