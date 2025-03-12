using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PearlArea : MonoBehaviour
{
    [SerializeField] private List<Pearl> pearlObjects = new List<Pearl>();

    [SerializeField] private float percentRequired = 0.75f;

    public UnityEvent OnThresholdReached;
    
    private int totalCount = 0;
    
    private void Start()
    {
        totalCount = pearlObjects.Count;
        foreach (Pearl p in pearlObjects){
            p.pearlArea = this;
        }
    }

    public void RemovePearl(Pearl p)
    {
        Debug.Log(p.name);
        if (pearlObjects.Contains(p)){
            pearlObjects.Remove(p);
            CheckThreshold();
        }
    }

    private void CheckThreshold()
    {
        if ((pearlObjects.Count / (float) totalCount) <= percentRequired){
            Debug.Log($"Pearl Area {gameObject.name} Reached");
            OnThresholdReached?.Invoke();
            // Change highest blood level.
        }
    }
    
    
    
}