using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphincterDoor : MonoBehaviour
{
    Animator animator;
    Collider collider;

    private bool _isOpen = false;
    
    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        collider = GetComponentInChildren<Collider>();
    }

    public void Open()
    {
        animator.SetTrigger("Open");
        collider.enabled = false;
        
    }

    public void Close()
    {
        
        animator.SetTrigger("Close");
        collider.enabled = true;
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
