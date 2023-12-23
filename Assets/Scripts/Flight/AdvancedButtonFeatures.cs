using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class AdvancedButtonFeatures : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Button button;
    public void OnPointerDown(PointerEventData eventData)
    {
        if(!button.enabled) SoundManager.Instance.PlaySoundByName("InvalidAction");
    }
}

