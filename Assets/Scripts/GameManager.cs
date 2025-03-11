using System;
using Scripts.Utilities;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : Singleton<GameManager>
{
    public int PearlCount { get; private set; } = 0;

    public float CurrentBloodLevel => _risingBlood.CurrentBloodLevel;

    private RisingBlood _risingBlood;

    public bool paused = false;
    
    private void Start()
    {
        _risingBlood = FindObjectOfType<RisingBlood>();
    }

    public void AddPearl(int amount = 1)
    {
        PearlCount += amount;
    }
}