using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSlider : MonoBehaviour
{
    private float sliderValue;
    public float SliderValue
    {
        get{return sliderValue;}
        set
        {
            if (value == sliderValue) return;
            sliderValue = value;
            OnSliderValueChanged?.Invoke(value);
        }
    }
    public delegate void OnSliderValueChangedDelegate(float value);
    public event OnSliderValueChangedDelegate OnSliderValueChanged;

    [SerializeField] private Slider slider;

    public float startingValue = -1;
    public float maxSliderValue;
    public float minSliderValue;


    private void Awake()
    {
        OnSliderValueChanged += UpdateSliderUI;
        UpdateSliderUI(startingValue);
        slider.interactable = false;
    }

    private void UpdateSliderUI(float value)
    {
        slider.maxValue = maxSliderValue;
        slider.minValue = minSliderValue;

        slider.value = value;
    }
}
