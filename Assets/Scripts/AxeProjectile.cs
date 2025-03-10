using System.Collections;
using System.Collections.Generic;
using Content.Scripts.Components;
using UnityEngine;

public class AxeProjectile : ProjectileComponent
{
    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        rb.isKinematic = true;
        GetComponentInChildren<InteractorComponent>().DeactivateInteractable();
    }
}
