using System;
using System.Collections.Generic;
using Scripts.Utilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class GameManager : Singleton<GameManager>
{
    public Trackable<int> PearlCount = new(0);

    public int totalPearlCount = 0;

    public float CurrentBloodLevel => _risingBlood.CurrentBloodLevel;

    private RisingBlood _risingBlood;

    public bool paused = false;

    public bool inTerminal = false;

    [SerializeField] public List<int> requiredPearls = new List<int>()
        {1, 2};

    [SerializeField] public List<UnityEvent> upgrades = new List<UnityEvent>();

    public int upgradesUnlocked = 0;

    private void Start()
    {
        _risingBlood = FindObjectOfType<RisingBlood>();
        Pearl[] allActivePearls = FindObjectsOfType<Pearl>();
        totalPearlCount = allActivePearls.Length;
    }

    public void AddPearl(int amount = 1)
    {
        PearlCount.Value += amount;
    }


    public void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        paused = true;
        Time.timeScale = 0f;
    }

    public void Unpause()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1.0f;
        paused = false;
    }

    public void Upgrade()
    {
        if (upgradesUnlocked < upgrades.Count){
            upgrades[upgradesUnlocked]?.Invoke();
        }
        upgradesUnlocked++;
    }
}