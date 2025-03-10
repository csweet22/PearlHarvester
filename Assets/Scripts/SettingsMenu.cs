using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Scripts.Utilities;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : Singleton<SettingsMenu>
{
    [SerializeField] private Slider sensitivitySlider;
    
    public float Sensitivity => sensitivitySlider.value;
}