﻿using System.Collections;
using TMPro;
using UnityEngine;

public class Message : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;

    public void SetText(string text)
    {
        messageText.text = text;
        StartCoroutine(DelayedDestroy(Mathf.Max(0.5f * text.Split(" ").Length, 1.0f)));
    }

    IEnumerator DelayedDestroy(float delay = 1.0f)
    {
        yield return new WaitForSecondsRealtime(delay);
        Destroy(this.gameObject);
    }
}