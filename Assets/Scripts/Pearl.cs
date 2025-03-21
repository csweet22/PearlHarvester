using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class Pearl : MonoBehaviour
{
    private PearlArea pearlArea;
    
    private OnetimeTrigger _onetimeTrigger;

    [SerializeField] private AudioClip getSound;
    
    // Start is called before the first frame update
    void Start()
    {
        _onetimeTrigger = GetComponentInChildren<OnetimeTrigger>();
        _onetimeTrigger.OnTriggered += EatPearl;
    }

    public void SetPearlArea(PearlArea newPearlArea)
    {
        this.pearlArea = newPearlArea;
    }

    private void EatPearl()
    {
        pearlArea?.RemovePearl(this);
        DOTween.To(() => gameObject.transform.position, x => gameObject.transform.position = x,
            PlayerCore.Instance.PlayerPosition, 0.2f).onComplete += () =>
        {
            AudioManager.Instance.SpawnSound(getSound);
            GameManager.Instance.AddPearl();
            Destroy(gameObject);
        };
    }
}