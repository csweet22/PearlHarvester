using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class TriggerComponent : MonoBehaviour
{
    public enum Target
    {
        Player,
        Enemy
    }

    [SerializeField] protected Target target;

    public event Action<Collider> TriggerStay;
    public event Action<Collider> TriggerEnter;
    public event Action<Collider> TriggerExit;

    public UnityEvent onEnter;
    public UnityEvent onStay;
    public UnityEvent onExit;

    protected Collider triggerCollider;

    protected virtual void Awake()
    {
        if (TryGetComponent<Collider>(out Collider triggerColliderOut))
            triggerCollider = triggerColliderOut;
        triggerCollider.isTrigger = true;
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        TriggerStay?.Invoke(other);
        onStay?.Invoke();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        // Debug.Log($"{name} hit {other.name}");
        TriggerEnter?.Invoke(other);
        onEnter?.Invoke();
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        TriggerExit?.Invoke(other);
        onExit?.Invoke();
    }

    public void SetLayers(LayerMask layer, LayerMask mask)
    {
        gameObject.layer = StaticHelpers.GetLayerIndex(layer);
        triggerCollider.includeLayers = mask;
    }

    private void OnEnable()
    {
        triggerCollider.enabled = true;
    }

    private void OnDisable()
    {
        triggerCollider.enabled = false;
    }
}