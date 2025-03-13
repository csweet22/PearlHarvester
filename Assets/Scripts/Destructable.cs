using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    private HealthComponent _healthComponent;
    private HealthboxComponent _healthbox;

    // Start is called before the first frame update
    void Start()
    {
        _healthbox = GetComponentInChildren<HealthboxComponent>();
        _healthComponent = GetComponentInChildren<HealthComponent>();

        _healthbox.OnHit += delta => { _healthComponent.ChangeHealth(delta); };

        _healthComponent.OnHealthEmpty += () => { Destroy(this.gameObject); };
    }

    private void OnDestroy()
    {
        AxeProjectile[] projectiles = GetComponentsInChildren<AxeProjectile>();
        foreach (AxeProjectile projectile in projectiles){
            Debug.Log($"parent to null: {projectile.name}");
            projectile.transform.parent = null;
        }
    }
}