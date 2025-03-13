using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthChangeBoxComponent : TriggerComponent
{
    public enum HealthChangeType
    {
        Damages,
        Heals
    }

    [SerializeField] private HealthChangeType healthChangeType;

    public event Action<HealthboxComponent> OnHit;

    [SerializeField] private int healthDelta = 5;

    [SerializeField] private float tickDuration = 1.0f;

    [SerializeField] public int tier = 1;

    private float _tickTime = 0.0f;

    public int HealthDelta => healthDelta;

    public bool willAffect = true;
    
    protected override void Awake()
    {
        base.Awake();
        SetTarget(target);
    }

    public void SetTarget(Target newTarget)
    {
        if (target == Target.Player)
            gameObject.layer = LayerMask.NameToLayer("ChangesPlayerHealth");
        else
            gameObject.layer = LayerMask.NameToLayer("ChangesEnemyHealth");
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        _tickTime = 0.0f;
    }

    public void ResetTickTimer()
    {
        SetTickTime(0.0f);
    }

    public void SetTickTime(float time)
    {
        _tickTime = time;
    }

    protected override void OnTriggerStay(Collider other)
    {
        if (!willAffect)
            return;
        base.OnTriggerStay(other);
        HealthboxComponent healthboxComponent = other.GetComponent<HealthboxComponent>();

        int actualHealthDelta = (healthChangeType == HealthChangeType.Heals) ? HealthDelta : -HealthDelta;

        if (healthboxComponent != null){
            if (_tickTime == 0.0f){
                if (tier >= healthboxComponent.Tier){
                    healthboxComponent.ChangeHealth(actualHealthDelta);
                }
                OnHit?.Invoke(healthboxComponent);
            }

            _tickTime += Time.deltaTime;
            if (_tickTime > tickDuration)
                _tickTime = 0.0f;
        }
    }
}