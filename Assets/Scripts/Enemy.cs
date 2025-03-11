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

    private void Start()
    {
        _rigidbody = GetComponentInChildren<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 currentVelocity = _rigidbody.velocity;
        _targetVelocity.y = _rigidbody.velocity.y;
        Vector3 deltaVelocity = _targetVelocity - currentVelocity;
        _rigidbody.AddForce(deltaVelocity, ForceMode.VelocityChange);
        
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
        Debug.Log(_targetVelocity);
    }

    public void EndPursuit()
    {
        _targetVelocity = Vector3.zero;
    }
}