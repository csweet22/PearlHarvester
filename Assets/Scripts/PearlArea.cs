using System;
using System.Collections.Generic;
using UnityEngine;

public class PearlArea : MonoBehaviour
{
    [SerializeField] private List<Pearl> pearlObjects = new List<Pearl>();

    [SerializeField] private float percentRequired = 0.75f;

    private int totalCount = 0;
    
    private void Start()
    {
        totalCount = pearlObjects.Count;
    }

    private void CheckThreshold()
    {
        if (pearlObjects.Count == 0) return;
    }
    
}