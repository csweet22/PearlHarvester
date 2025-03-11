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

    [SerializeField] private float sprintFovIncrease = 7f;
    
    private void Start()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        LoadDefaultFOV();
    }

    public void LoadDefaultFOV(float duration = 0.2f)
    {
        SetFOV(Settings.Instance.fov, duration);
    }

    public void LoadSprintFOV(float duration = 0.3f)
    {
        SetFOV(Settings.Instance.fov + sprintFovIncrease, duration);
    }

    public void SetFOV(float fov = 90f, float duration = 1.0f)
    {
        DOTween.To(()=> _virtualCamera.m_Lens.FieldOfView, x=> _virtualCamera.m_Lens.FieldOfView = x, fov, duration).SetUpdate(true);
    }
}
