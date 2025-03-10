using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private TriggerComponent triggerComponent;
    [SerializeField] private Transform target;

    private void Start()
    {
        triggerComponent.TriggerEnter += Teleport;
    }

    private void Teleport(Collider col)
    {
        Rigidbody rb = col.GetComponentInParent<Rigidbody>();
        rb.gameObject.transform.position = target.position;
    }
}