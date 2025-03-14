using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneDoor : MonoBehaviour
{
    [SerializeField] private Material doorMat;


    HealthComponent healthComponent;

    private void Start()
    {
        healthComponent = GetComponentInChildren<HealthComponent>();
        healthComponent.OnLoseHealth += i => { StartCoroutine(Flash()); };
    }

    IEnumerator Flash()
    {
        doorMat.SetColor("_Emission", Color.red);
        yield return new WaitForSeconds(0.1f);
        doorMat.SetColor("_Emission", Color.black);
    }
}