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

    private void Start()
    {
        _targetBloodLevel = CurrentBloodLevel;
    }

    public void SetBloodSpeed(float newSpeed)
    {
        _risingSpeed = newSpeed;
    }

    public void SetBloodLevel(float level)
    {
        _targetBloodLevel = level;
        float distance = Mathf.Abs(bloodObject.localPosition.y - _targetBloodLevel);
        float duration = distance / _risingSpeed;
        Vector3 targetLocalPosition = bloodObject.localPosition.Change(y: _targetBloodLevel);
        DOTween.To(() => bloodObject.localPosition, x => bloodObject.localPosition = x, targetLocalPosition, duration);
    }
}