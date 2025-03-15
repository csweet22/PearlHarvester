using System;
using System.Collections.Generic;
using Scripts.Utilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

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

    [SerializeField] public Transform safeAreaSpawn;

    [SerializeField] private bool riseBloodOnStart = true;

    [SerializeField] public int quota = 10;

    public UnityEvent OnQuotaReached;
    public event Action onQuotaReached;

    public bool quotaReached = false;

    [SerializeField] public AudioClip whale1;
    [SerializeField] public AudioClip whale2;
    [SerializeField] public AudioClip whale3;

    [SerializeField] public AudioSource whaleSource;


    public float _runTime = 0.0f;
    private bool _gameEnded = false;

    public void PlayGroan()
    {
        Debug.Log("Play Groan");
        if (whaleSource.isPlaying)
            return;

        int rand = Random.Range(0, 3);
        switch (rand){
            case 0:
                whaleSource.clip = whale1;
                break;
            case 1:
                whaleSource.clip = whale2;
                break;
            case 2:
                whaleSource.clip = whale3;
                break;
        }

        Debug.Log("Actually played");
        whaleSource.Play();
    }

    public void EndGame()
    {
        _gameEnded = true;
    }
    
    private void Start()
    {
        _risingBlood = FindObjectOfType<RisingBlood>();
        if (riseBloodOnStart)
            _risingBlood?.GoToHighest();
        Pearl[] allActivePearls = FindObjectsOfType<Pearl>();
        totalPearlCount = allActivePearls.Length;
        HUD.Instance.UpdateQuotaAndTotal();
        _runTime = 0.0f;
    }

    private void Update()
    {
        if (!_gameEnded)
            _runTime += Time.deltaTime;
    }

    public void SetSafeAreaSpawn(Transform spawnPoint)
    {
        safeAreaSpawn = spawnPoint;
    }

    public void AddPearl(int amount = 1)
    {
        PearlCount.Value += amount;
        if (!quotaReached){
            if (PearlCount.Value >= quota){
                OnQuotaReached?.Invoke();
                onQuotaReached?.Invoke();
                quotaReached = true;
            }
        }
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