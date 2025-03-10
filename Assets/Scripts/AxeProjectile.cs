using System;
using System.Collections;
using System.Collections.Generic;
using Content.Scripts.Components;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class AxeProjectile : ProjectileComponent
{
    [SerializeField] private InputActionReference recallAction;
    [SerializeField] private Collider physicsCollider;

    private void OnEnable()
    {
        recallAction.action.Enable();

        recallAction.action.performed += OnRecallPerformed;
    }

    private void OnRecallPerformed(InputAction.CallbackContext obj)
    {
        // Turn of physics collider
        physicsCollider.enabled = false;

        // Lerp towards player
        Tween lerp = DOTween.To(() => gameObject.transform.position, x => gameObject.transform.position = x,
            PlayerCore.Instance.PlayerPosition, 0.5f);
        // Once at the player, increment axes & despawn
        lerp.onComplete += () =>
        {
            PlayerCore.Instance.AddAxe();

            recallAction.action.performed -= OnRecallPerformed;
            Destroy(gameObject);
        };
    }

    private void OnDisable()
    {
        recallAction.action.Disable();

        recallAction.action.performed -= OnRecallPerformed;
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        rb.isKinematic = true;
        GetComponentInChildren<InteractorComponent>().DeactivateInteractable();
    }
}