using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

//Custom UI Element using the new input system.
//Created to have more control over UI elements in code,
public class CustomUIElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] public RectTransform uiTransform;

    private bool isMouseOver = false;
    public bool IsMouseOver
    {
        get
        {
            return isMouseOver;
        }
        set
        {
            if(isMouseOver == value) return;
            isMouseOver = value;
            OnMouseOverChanged?.Invoke(isMouseOver);
        }
    }
    public delegate void OnMouseOverChangedDelegate(bool mouseOver);
    public event OnMouseOverChangedDelegate OnMouseOverChanged;

    protected virtual void Awake()
    {
        if(uiTransform == null) uiTransform = GetComponent<RectTransform>();
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        IsMouseOver = true;
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        IsMouseOver = false;
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        OnLMBClicked?.Invoke();
    }
    public delegate void OnLMBClickedDelegate();
    public event OnLMBClickedDelegate OnLMBClicked;

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        OnLMBReleased?.Invoke();
    }
    public delegate void OnLMBReleasedDelegate();
    public event OnLMBReleasedDelegate OnLMBReleased;
}
