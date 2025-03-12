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
        _triggerComponent.TriggerEnter += EatPearl;
    }

    private void EatPearl(Collider obj)
    {
        _triggerComponent.enabled = false;
        DOTween.To(() => gameObject.transform.position, x => gameObject.transform.position = x,
            PlayerCore.Instance.PlayerPosition, 0.2f).onComplete += () =>
        {
            GameManager.Instance.AddPearl();
            Destroy(gameObject);
        };
    }
}