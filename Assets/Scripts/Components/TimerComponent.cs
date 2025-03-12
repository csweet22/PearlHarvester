using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class TimerComponent : MonoBehaviour
{
    [SerializeField] private float duration = 1.0f;

    private float _currentTime = 0.0f;

    [SerializeField] private bool isLooping = true;

    [SerializeField] private bool autoStart = true;

    private bool _isPaused = false;

    public UnityEvent OnTimeoutEvent;
    public event Action OnTimeoutAction;

    private void Awake()
    {
        _isPaused = !autoStart;
    }

    public void StartTimer(float position = 0.0f)
    {
        _currentTime = position;
        _isPaused = false;
    }

    public void PauseTimer()
    {
        _isPaused = true;
    }

    public void StopTimer()
    {
        _isPaused = false;
        _currentTime = 0.0f;
    }

    public void Restart()
    {
        StopTimer();
        StartTimer();
    }

    private void Update()
    {
        if (_isPaused)
            return;

        _currentTime += Time.deltaTime;

        if (_currentTime <= duration)
            return;

        OnTimeoutEvent?.Invoke();
        OnTimeoutAction?.Invoke();

        if (isLooping){
            StartTimer(0.0f);
        }
        else{
            PauseTimer();
        }
    }
}