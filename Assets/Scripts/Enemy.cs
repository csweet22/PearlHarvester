using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float faceDuration = 1.0f;
    [SerializeField] private float waitBeforeBullDuration = 0.5f;
    [SerializeField] private float waitAfterStoppedDuration = 0.5f;
    
    private Rigidbody _rigidbody;

    private Vector3 _targetVelocity = Vector3.zero;

    [SerializeField] private int updateFrequency = 10;
    private int _tick = 0;

    [SerializeField] private GameObject mesh;

    // [SerializeField] 
    private float rotateSpeed = 5f;

    private HealthComponent _healthComponent;
    private HealthboxComponent _healthboxComponent;

    private Coroutine _stateCoroutine;
    private Tween _rotateTween;

    private void Start()
    {
        _healthComponent = GetComponentInChildren<HealthComponent>();
        _healthComponent.OnHealthEmpty += Die;
        _healthboxComponent = GetComponentInChildren<HealthboxComponent>();
        _healthboxComponent.OnHit += delta => { _healthComponent.ChangeHealth(delta); };
        _rigidbody = GetComponentInChildren<Rigidbody>();
    }

    private bool _seesPlayer = false;

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
    }

    public void PlayerSpotted()
    {
        if (_seesPlayer)
            return;
        _seesPlayer = true;
        _stateCoroutine = StartCoroutine(FaceTowardsPlayer());
    }

    IEnumerator FaceTowardsPlayer()
    {
        Vector3 direction = (PlayerCore.Instance.PlayerPosition - transform.position).Change(y: 0).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        _rotateTween = mesh.transform.DORotate(targetRotation.eulerAngles, faceDuration, RotateMode.Fast);

        yield return new WaitForSeconds(faceDuration + waitBeforeBullDuration);

        _targetVelocity = (direction * speed);
    }

    private void OnDestroy()
    {
        if (_stateCoroutine != null)
            StopCoroutine(_stateCoroutine);
        if (_rotateTween != null)
            _rotateTween.Kill();
    }

    public void PlayerLost()
    {
        Debug.Log("Player lost");
        _seesPlayer = false;
    }

    public void Bumped()
    {
        _targetVelocity = Vector3.zero;
        StartCoroutine(BumpWait());
    }

    IEnumerator BumpWait()
    {
        yield return new WaitForSeconds(waitAfterStoppedDuration);
        if (_seesPlayer){
            _stateCoroutine = StartCoroutine(FaceTowardsPlayer());
        }
    }
}