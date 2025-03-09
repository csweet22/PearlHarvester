using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    private CinemachinePOV _pov;

    void Start()
    {
        sensitivitySlider.onValueChanged.AddListener(OnSliderValueChanged);
        _pov = virtualCamera.GetCinemachineComponent<CinemachinePOV>();
    }

    private void OnSliderValueChanged(float value)
    {
        _pov.m_HorizontalAxis.m_MaxSpeed = value;
        _pov.m_VerticalAxis.m_MaxSpeed = value;
    }
}