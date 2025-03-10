using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerComponent : MonoBehaviour
{
    [SerializeField] private GameObject spawnerPrefab;
    [SerializeField] private Transform spawnerTransform;

    public void Spawn()
    {
        GameObject spawned = Instantiate(spawnerPrefab);
        spawned.transform.position = spawnerTransform.position;
        spawned.transform.rotation = spawnerTransform.rotation;
    }
    
}
