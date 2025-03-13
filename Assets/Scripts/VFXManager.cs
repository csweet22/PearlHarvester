using System.Collections;
using System.Collections.Generic;
using Scripts.Utilities;
using UnityEngine;
using UnityEngine.VFX;

public class VFXManager : Singleton<VFXManager>
{
    public void SpawnVFX(VisualEffectAsset vfxAsset, float duration = 1.0f, Vector3 position = default(Vector3))
    {
        GameObject obj = new GameObject
        {
            transform =
            {
                position = position
            }
        };
        obj.transform.SetParent(transform);
        VisualEffect effect = obj.AddComponent<VisualEffect>();
        effect.visualEffectAsset = vfxAsset;
        effect.Play();
        StartCoroutine(
            DelayedDestroy(obj, duration));
    }

    IEnumerator DelayedDestroy(GameObject obj, float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(obj);
    }
}