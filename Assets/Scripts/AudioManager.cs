using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Scripts.Utilities;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    public AudioMixer mixer;

    private Tween _lowpassTween;

    private float lowpassValue = 22000f;

    protected override void Awake()
    {
        base.Awake();
        mixer = Resources.Load<AudioMixer>("LevelMixer");
    }

    public void SubmergeAudio()
    {
        Debug.Log("SubmergeAudio");
        if (_lowpassTween != null)
            _lowpassTween.Kill();
        _lowpassTween = DOTween.To(() => lowpassValue, x => lowpassValue = x, 1000f, 1.0f);
        _lowpassTween.onUpdate += () => { mixer.SetFloat("Lowpass", lowpassValue); };
    }

    public void DesubmergeAudio()
    {
        Debug.Log("DesubmergeAudio");
        if (_lowpassTween != null)
            _lowpassTween.Kill();
        _lowpassTween = DOTween.To(() => lowpassValue, x => lowpassValue = x, 22000f, 1.0f);
        _lowpassTween.onUpdate += () => { mixer.SetFloat("Lowpass", lowpassValue); };
    }

    public void SpawnSound(AudioClip clip, float duration = 0.0f)
    {
        GameObject obj = new GameObject();
        AudioSource source = obj.AddComponent<AudioSource>();
        source.clip = clip;
        source.Play();
        source.outputAudioMixerGroup = mixer.FindMatchingGroups("Level")[0];

        float actualDuration = duration;
        if (duration == 0.0f)
            actualDuration = source.clip.length;

        StartCoroutine(
            DelayedDestroy(obj, actualDuration));
    }

    IEnumerator DelayedDestroy(GameObject obj, float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(obj);
    }
}