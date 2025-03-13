using System.Collections;
using System.Collections.Generic;
using Scripts.Utilities;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public void SpawnSound(AudioClip clip, float duration = 0.0f)
    {
        GameObject obj = new GameObject();
        AudioSource source = obj.AddComponent<AudioSource>();
        source.clip = clip;
        source.Play();
        
        float actualDuration = duration;
        if (duration == 0.0f)
            actualDuration = source.clip.length;
        
        StartCoroutine(
            DelayedDestroy(obj, actualDuration));
    }

    IEnumerator DelayedDestroy(GameObject obj, float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(obj);
    }
}