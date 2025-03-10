using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileComponent : MonoBehaviour
{
    protected Vector3 currentVelocity = Vector3.zero;

    protected event Action Timeout;
    protected event Action OnDisappear;

    protected Rigidbody rb;
    protected HealthboxComponent _healthboxComponent;
    protected HealthChangeBoxComponent healthChangeBoxComponent;

    protected float lifetime;
    protected Vector3 gravity;
    protected Vector3 acceleration;
    protected bool destroyOnTimeout;
    protected bool destroyOnPhysicsCollision;
    protected bool destroyOnHealthboxCollision;
    protected bool destroyOnDamageboxCollision;
    protected float drag;
    protected Vector3 initialVelocity;

    private void Awake()
    {
        Timeout += OnTimeout;
        rb = GetComponent<Rigidbody>();

        _healthboxComponent = GetComponentInChildren<HealthboxComponent>();
        _healthboxComponent.OnHit += HealthboxComponentOnHit;

        healthChangeBoxComponent = GetComponentInChildren<HealthChangeBoxComponent>();
        healthChangeBoxComponent.OnHit += HealthChangeBoxComponentOnOnHit;
    }

    public void Init(ProjectileData projectileData, Vector3 startingVelocity,
        LauncherComponent.LauncherTeam launcherTeam)
    {
        StartCoroutine(LifeTimer(projectileData.lifetime));
        gravity = projectileData.gravity;
        acceleration = projectileData.acceleration;
        destroyOnTimeout = projectileData.destroyOnTimeout;
        destroyOnPhysicsCollision = projectileData.destroyOnPhysicsCollision;
        destroyOnHealthboxCollision = projectileData.destroyOnHealthboxCollision;
        destroyOnDamageboxCollision = projectileData.destroyOnDamageboxCollision;
        drag = projectileData.drag;

        initialVelocity = startingVelocity;
        currentVelocity = startingVelocity;
        rb.AddForce(currentVelocity, ForceMode.VelocityChange);
        rb.drag = drag;

        if (launcherTeam == LauncherComponent.LauncherTeam.Player){
            _healthboxComponent.SetTeam(TriggerComponent.Target.Player);
            healthChangeBoxComponent.SetTarget(TriggerComponent.Target.Player);
        }
        else{
            _healthboxComponent.SetTeam(TriggerComponent.Target.Enemy);
            healthChangeBoxComponent.SetTarget(TriggerComponent.Target.Enemy);
        }
    }

    private void OnTimeout()
    {
        if (destroyOnTimeout)
            Disappear();
    }

    private void HealthboxComponentOnHit(int damage)
    {
        if (destroyOnHealthboxCollision)
            Disappear();
    }

    private void HealthChangeBoxComponentOnOnHit(HealthboxComponent healthboxComponent)
    {
        if (destroyOnDamageboxCollision)
            Disappear();
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (destroyOnPhysicsCollision)
            Disappear();
    }

    protected IEnumerator LifeTimer(float duration)
    {
        if (duration > 0.0f){
            yield return new WaitForSeconds(duration);
            if (!_disappearCalled)
                Timeout?.Invoke();
        }
    }

    private bool _disappearCalled = false;

    protected virtual void Disappear()
    {
        if (!_disappearCalled){
            OnDisappear?.Invoke();
            Destroy(gameObject);
        }
        else{
            Debug.LogError("Called Disappear twice.");
        }
    }

    private void FixedUpdate()
    {
        Tick();
    }

    protected virtual void Tick()
    {
        rb.AddForce(gravity, ForceMode.Force);
    }
}