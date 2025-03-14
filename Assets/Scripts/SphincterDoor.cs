using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphincterDoor : MonoBehaviour
{
    private Animator _animator;
    private Collider _col;

    private bool _isOpen = false;

    private AudioSource audioSource;
    
    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _col = GetComponentInChildren<Collider>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Open()
    {
        _animator.SetTrigger("Open");
        _col.enabled = false;
        audioSource.Play();
        
    }

    public void Close()
    {
        
        _animator.SetTrigger("Close");
        _col.enabled = true;
        audioSource.Play();
    }
    
    public void ToggleDoor()
    {
        if (_isOpen){
            Close();
        }
        else{
            Open();
        }
        _isOpen = !_isOpen;
    }
}
