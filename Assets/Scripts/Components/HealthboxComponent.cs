using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HealthboxComponent : TriggerComponent
{
    public event Action<int> OnHit;

    protected override void Awake()
    {
        base.Awake();
        SetTeam(target);
    }

    public void SetTeam(Target newOwner)
    {
        if (target == Target.Player)
            gameObject.layer = LayerMask.NameToLayer("PlayerHealthbox");
        else
            gameObject.layer = LayerMask.NameToLayer("EnemyHealthbox");
    }

    public void ChangeHealth(int healthDelta)
    {
        OnHit?.Invoke(healthDelta);
    }
}