using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    private Rigidbody _rigidbody;

    private Vector3 _targetVelocity = Vector3.zero;

    [SerializeField] private int updateFrequency = 10;
    private int _tick = 0;

    [SerializeField] private GameObject mesh;

    [SerializeField] private float rotateSpeed = 5f;

    private HealthComponent _healthComponent;
    private HealthboxComponent _healthboxComponent;

    private void Start()
    {
        _healthComponent = GetComponentInChildren<HealthComponent>();
        _healthComponent.OnHealthEmpty += Die;
        _healthboxComponent = GetComponentInChildren<HealthboxComponent>();
        _healthboxComponent.OnHit += delta => { _healthComponent.ChangeHealth(delta); };
        _rigidbody = GetComponentInChildren<Rigidbody>();
    }

    private void Die()
    {
        Destroy(this.gameObject);
    }

    private void FixedUpdate()
    {
        Vector3 currentVelocity = _rigidbody.velocity;
        _targetVelocity.y = _rigidbody.velocity.y;
        Vector3 actualVelocity = Vector3.Slerp(currentVelocity, _targetVelocity, Time.deltaTime * rotateSpeed);
        Vector3 deltaVelocity = actualVelocity - currentVelocity;
        _rigidbody.AddForce(deltaVelocity, ForceMode.VelocityChange);

        if (_targetVelocity.Change(y: 0f) != Vector3.zero){
            Quaternion targetRotation = Quaternion.LookRotation(_targetVelocity.normalized);
            mesh.transform.rotation =
                Quaternion.Slerp(mesh.transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);
        }
    }

    public void StartPursuit()
    {
        UpdateTargetVelocity();
    }

    public void UpdatePursuit()
    {
        _tick++;
        if (_tick > updateFrequency){
            _tick = 0;
            UpdateTargetVelocity();
        }
    }

    private void UpdateTargetVelocity()
    {
        Vector3 direction = (PlayerCore.Instance.PlayerPosition - transform.position).normalized;
        _targetVelocity = (direction * speed);
    }

    public void EndPursuit()
    {
        _targetVelocity = Vector3.zero;
    }
}