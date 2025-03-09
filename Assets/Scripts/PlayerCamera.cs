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

    private void Start()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void SetFOV(float fov = 90f, float duration = 1.0f)
    {
        DOTween.To(()=> _virtualCamera.m_Lens.FieldOfView, x=> _virtualCamera.m_Lens.FieldOfView = x, fov, duration);
    }
}
