using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using Scripts.Utilities;
using UnityEngine;

public class PlayerCamera : Singleton<PlayerCamera>
{
    private CinemachineVirtualCamera _virtualCamera;

    
    [SerializeField] private float defaultFov = 75f;
    [SerializeField] private float sprintFov = 80f;
    
    private void Start()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        LoadDefaultFOV(0.0f);
    }

    public void LoadDefaultFOV(float duration = 0.2f)
    {
        SetFOV(defaultFov, duration);
    }

    public void LoadSprintFOV(float duration = 0.3f)
    {
        SetFOV(sprintFov, duration);
    }

    public void SetFOV(float fov = 90f, float duration = 1.0f)
    {
        DOTween.To(()=> _virtualCamera.m_Lens.FieldOfView, x=> _virtualCamera.m_Lens.FieldOfView = x, fov, duration);
    }
}
