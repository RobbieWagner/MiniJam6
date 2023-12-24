using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConspiracySlider : MonoBehaviour
{
    [SerializeField] private GameSlider slider;

    [SerializeField] private Image handle;
    [SerializeField] private Image fill;

    [SerializeField] private Color[] colors;

    private void Awake()
    {
        slider.OnSliderValueChanged += UpdateUIImage;
        UpdateUIImage(0);
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
       fill.color = colors[v];
    }
}
