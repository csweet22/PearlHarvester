using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class Pearl : MonoBehaviour
{
    private TriggerComponent _triggerComponent;

    // Start is called before the first frame update
    void Start()
    {
        _triggerComponent = gameObject.GetComponentInChildren<TriggerComponent>();
        _triggerComponent.TriggerEnter += OnTriggerEnter;
    }

    private void OnTriggerEnter(Collider obj)
    {
        Tween lerp = DOTween.To(() => gameObject.transform.position, x => gameObject.transform.position = x,
            PlayerCore.Instance.PlayerPosition, 0.2f);
        lerp.onComplete += () =>
        {
            GameManager.Instance.AddPearl();
            Destroy(gameObject);
        };
    }
}