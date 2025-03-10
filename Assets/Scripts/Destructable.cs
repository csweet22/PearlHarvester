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

        _healthbox.OnHit += delta =>
        {
            Debug.Log(delta);
            _healthComponent.ChangeHealth(delta);
        };

        _healthComponent.OnHealthEmpty += () => { Destroy(this.gameObject); };
    }
}