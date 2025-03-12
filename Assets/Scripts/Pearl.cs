using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class Pearl : MonoBehaviour
{
    private TriggerComponent _triggerComponent;

    private PearlArea pearlArea;
    
    // Start is called before the first frame update
    void Start()
    {
        _triggerComponent = gameObject.GetComponentInChildren<TriggerComponent>();
        _triggerComponent.TriggerEnter += EatPearl;
    }

    public void SetPearlArea(PearlArea newPearlArea)
    {
        Debug.Log(newPearlArea.gameObject.name);
        this.pearlArea = newPearlArea;
    }

    private void EatPearl(Collider obj)
    {
        pearlArea?.RemovePearl(this);
        _triggerComponent.enabled = false;
        DOTween.To(() => gameObject.transform.position, x => gameObject.transform.position = x,
            PlayerCore.Instance.PlayerPosition, 0.2f).onComplete += () =>
        {
            GameManager.Instance.AddPearl();
            Destroy(gameObject);
        };
    }
}