using System.Collections;
using System.Collections.Generic;
using Scripts.Utilities;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public void SpawnVFX(AudioClip clip, float duration = 1.0f)
    {
        GameObject obj = new GameObject();
        obj.transform.SetParent(transform);
        AudioSource source = obj.AddComponent<AudioSource>();
        source.clip = clip;
        source.Play();
        StartCoroutine(
            DelayedDestroy(obj, duration));
    }

    IEnumerator DelayedDestroy(GameObject obj, float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(obj);
    }
}