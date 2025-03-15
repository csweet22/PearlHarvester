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
    [SerializeField] private float _risingSpeed = 0.4f;

    [SerializeField] private float _regularDelay = 1.0f;
    
    [SerializeField] public float _lowestHeight = -100f;
    [SerializeField] public float _highestHeight = 0f;

    private Coroutine _risingCoroutine;
    private Tween _tween;

    public bool IsRising = false;

    public bool IsSubmerged => PlayerCore.Instance.PlayerPosition.y + 1f < bloodObject.position.y;

    public void SetHighestHeight(float newHighestHeight)
    {
        _highestHeight = newHighestHeight;
    }

    public void SetLowestHeight(float newLowestHeight)
    {
        _lowestHeight = newLowestHeight;
    }

    private void Start()
    {
        _targetBloodLevel = CurrentBloodLevel;
        GameManager.Instance.onQuotaReached += OnQuotaReached;
    }

    private void OnQuotaReached()
    {
        if (_tween != null)
            _tween.Kill();
        if (_risingCoroutine != null)
            StopCoroutine(_risingCoroutine);

        _targetBloodLevel = _highestHeight;
        float distance = Mathf.Abs(bloodObject.localPosition.y - _targetBloodLevel);
        float duration = distance / _risingSpeed;
        Vector3 targetLocalPosition = bloodObject.localPosition.Change(y: _targetBloodLevel);
        _tween = DOTween.To(() => bloodObject.localPosition, x => bloodObject.localPosition = x, targetLocalPosition,
            duration);
        _tween.onComplete +=
            () =>
            {
                _targetBloodLevel = _lowestHeight;
                float distance2 = Mathf.Abs(bloodObject.localPosition.y - _targetBloodLevel);
                float duration2 = distance2 / _risingSpeed;
                Vector3 targetLocalPosition2 = bloodObject.localPosition.Change(y: _targetBloodLevel);
                _tween = DOTween.To(() => bloodObject.localPosition, x => bloodObject.localPosition = x,
                    targetLocalPosition2,
                    duration2);
            };
    }

    public void SetBloodSpeed(float newSpeed)
    {
        _risingSpeed = newSpeed;
        SetBloodLevel(_targetBloodLevel);
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
        yield return new WaitForSeconds(_regularDelay);
        GoToLowest();
    }

    IEnumerator DelayHigh()
    {
        yield return new WaitForSeconds(_regularDelay);
        GoToHighest();
    }

    public void SetBloodLevel(float level)
    {
        _targetBloodLevel = level;
        float distance = Mathf.Abs(bloodObject.localPosition.y - _targetBloodLevel);
        float duration = distance / _risingSpeed;
        Vector3 targetLocalPosition = bloodObject.localPosition.Change(y: _targetBloodLevel);
        if (_tween != null)
            _tween.Kill();
        _tween = DOTween.To(() => bloodObject.localPosition, x => bloodObject.localPosition = x, targetLocalPosition,
            duration);
        _tween.onComplete +=
            () =>
            {
                if (_risingCoroutine != null)
                    StopCoroutine(_risingCoroutine);
                _risingCoroutine = StartCoroutine(IsRising ? DelayLow() : DelayHigh());
            };
    }
}