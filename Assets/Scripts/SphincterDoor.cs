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

    public void ToggleDoor()
    {
        if (_isOpen){
            animator.SetTrigger("Close");
            collider.enabled = true;
        }
        else{
            animator.SetTrigger("Open");
            collider.enabled = false;
            
        }
        _isOpen = !_isOpen;
    }
}
