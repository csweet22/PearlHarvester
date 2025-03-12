using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class RisingBlood : MonoBehaviour
{
    [SerializeField] private Transform bloodObject;

    public float CurrentBloodLevel => bloodObject.localPosition.y;
    private float _targetBloodLevel = 0f;
    private float _risingSpeed = 1f;

    [SerializeField] public float _lowestHeight = -100f;
    [SerializeField] public float _highestHeight = 0f;

    public bool IsRising = false;

    private void Start()
    {
        _targetBloodLevel = CurrentBloodLevel;
    }

    public void SetBloodSpeed(float newSpeed)
    {
        _risingSpeed = newSpeed;
    }

    public void GoToLowest()
    {
        IsRising = false;
        SetBloodLevel(_lowestHeight);
    }

    public void GoToHighest()
    {
        IsRising = true;
        SetBloodLevel(_highestHeight);
    }

    IEnumerator DelayLow()
    {
        yield return new WaitForSeconds(0.5f);
        GoToLowest();
    }
    
    IEnumerator DelayHigh()
    {
        yield return new WaitForSeconds(0.5f);
        GoToHighest();
    }
    
    public void SetBloodLevel(float level)
    {
        _targetBloodLevel = level;
        float distance = Mathf.Abs(bloodObject.localPosition.y - _targetBloodLevel);
        float duration = distance / _risingSpeed;
        Vector3 targetLocalPosition = bloodObject.localPosition.Change(y: _targetBloodLevel);
        DOTween.To(() => bloodObject.localPosition, x => bloodObject.localPosition = x, targetLocalPosition, duration)
                .onComplete +=
            () => { StartCoroutine(IsRising ? DelayLow() : DelayHigh()); };
    }
}