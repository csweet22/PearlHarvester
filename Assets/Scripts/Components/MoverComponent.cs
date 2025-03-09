using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[System.Serializable]
public struct MoverLocation
{
    public Transform transform;
    public float duration;
    public float delay;
    public Ease ease;
}

public class MoverComponent : MonoBehaviour
{
    [SerializeField] protected bool pingPong = false;

    [SerializeField] protected UpdateType updateType;

    [SerializeField] protected List<MoverLocation> locations;

    protected int incrementor = 1;
    protected int currentTargetLocation = -1;

    public virtual void GoToNext()
    {
    }

    protected MoverLocation GetNextLocation()
    {
        if (pingPong){
            if ((currentTargetLocation + incrementor) >= locations.Count || (currentTargetLocation + incrementor) < 0)
                incrementor *= -1;
        }

        currentTargetLocation += incrementor;
        currentTargetLocation = currentTargetLocation % locations.Count;

        return locations[currentTargetLocation];
    }
}