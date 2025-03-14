using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Scripts.Utilities;
using UnityEngine;
using UnityEngine.Serialization;

public class MusicManager : Singleton<MusicManager>
{
    [SerializeField] private AudioSource audioLeader;
    [SerializeField] private AudioSource audioFollower;

    private Tween _volumeTween;

    void Update()
    {
        audioFollower.timeSamples = audioLeader.timeSamples;
    }

    public void EnableDrums(float duration = 1.0f)
    {
        if (_volumeTween != null)
            _volumeTween.Kill();

        _volumeTween = DOTween.To(() => audioFollower.volume, x => audioFollower.volume = x, 1.0f, duration);
        _volumeTween.onComplete +=
            () => { StartCoroutine(DelayBeforeDisable()); };
    }

    private IEnumerator DelayBeforeDisable(float duration = 5f)
    {
        yield return new WaitForSeconds(duration);
        DisableDrums();
    }

    public void DisableDrums(float duration = 10.0f)
    {
        if (_volumeTween != null)
            _volumeTween.Kill();

        _volumeTween = DOTween.To(() => audioFollower.volume, x => audioFollower.volume = x, 0.0f, duration);
    }
}