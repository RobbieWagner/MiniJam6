using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class SatisfactionSlider : MonoBehaviour
{
    [SerializeField] private GameSlider slider;

    [SerializeField] private Image handle;
    [SerializeField] private Image fill;

    [SerializeField] private Color[] colors;
    [SerializeField] private Sprite[] sprites;

    private void Awake()
    {
        slider.OnSliderValueChanged += UpdateUIImage;
        UpdateUIImage(slider.maxSliderValue);
    }

    private void UpdateUIImage(float value)
    {
        if(value <= slider.maxSliderValue / 4)
        {
            SwitchSliderVisual(0);
        }
        else if(value <= slider.maxSliderValue / 2)
        {
            SwitchSliderVisual(1);
        }
        else
        {
            SwitchSliderVisual(2);
        }
    }

    private void SwitchSliderVisual(int v)
    {
       handle.color = colors[v];
       handle.sprite = sprites[v];
       fill.color = colors[v];
    }
}
