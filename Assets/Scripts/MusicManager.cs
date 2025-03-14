using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Utilities;
using UnityEngine;
using UnityEngine.Serialization;

public class MusicManager : Singleton<MusicManager>
{
    [SerializeField] private AudioSource audioLeader;
    [SerializeField] private AudioSource audioFollower;

    void Update() {
        audioFollower.timeSamples = audioLeader.timeSamples;
    }
    
    
}
