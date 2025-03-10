using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeProjectile : ProjectileComponent
{
    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        rb.isKinematic = true;
    }
}
