using System;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] public Trackable<float> Progress = new(0.5f);
    [SerializeField] public readonly Trackable<float> min = new(0.0f);
    [SerializeField] public readonly Trackable<float> max = new(1.0f);
    private Slider _slider;

    private void Start()
    {
        _slider = GetComponent<Slider>();
        
        _slider.minValue = min.Value;
        _slider.maxValue = max.Value;
        _slider.value = Progress.Value;
        
        min.OnValueChanged += (oldValue, newValue) => { _slider.minValue = newValue; };

        max.OnValueChanged += (oldValue, newValue) => { _slider.maxValue = newValue; };

        Progress.OnValueChanged += (oldValue, newValue) =>
        {
            if (newValue > max.Value){
                Progress.Value = max.Value;
                return;
            }

            if (newValue < min.Value){
                Progress.Value = min.Value;
                return;
            }

            _slider.value = Progress.Value;
        };
    }
}