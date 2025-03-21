﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PearlArea : MonoBehaviour
{
    [SerializeField] private List<Pearl> pearlObjects = new List<Pearl>();

    [SerializeField] private int pearlsRequired = 1;

    public UnityEvent OnThresholdReached;

    private int totalCount = 0;

    private bool _triggered = false;

    private void Start()
    {
        totalCount = pearlObjects.Count;
        foreach (Pearl p in pearlObjects){
            p.SetPearlArea(this);
        }
    }

    public void RemovePearl(Pearl p)
    {
        if (pearlObjects.Contains(p)){
            pearlObjects.Remove(p);
            CheckThreshold();
        }
    }

    private void CheckThreshold()
    {
        if (GameManager.Instance.quotaReached)
            return;
        
        if (_triggered)
            return;

        if ((totalCount - pearlObjects.Count) >= pearlsRequired){
            _triggered = true;
            Debug.Log($"Pearl Area {gameObject.name} Reached");
            OnThresholdReached?.Invoke();
            // Change highest blood level.
        }
    }
}