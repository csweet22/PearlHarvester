using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TransformMoverComponent : MoverComponent
{
    [SerializeField] protected Transform target;

    public override void GoToNext()
    {
        MoverLocation nextLocation = GetNextLocation();

        target.transform.DOMove(nextLocation.transform.position, nextLocation.duration).SetEase(nextLocation.ease)
            .SetDelay(nextLocation.delay).SetUpdate(updateType);
        target.transform.DORotate(nextLocation.transform.rotation.eulerAngles, nextLocation.duration)
            .SetEase(nextLocation.ease).SetDelay(nextLocation.delay).SetUpdate(updateType);
        target.transform.DOScale(nextLocation.transform.localScale, nextLocation.duration).SetEase(nextLocation.ease)
            .SetDelay(nextLocation.delay).SetUpdate(updateType);
    }
}