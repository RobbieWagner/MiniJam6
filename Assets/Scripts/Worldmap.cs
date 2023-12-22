using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Worldmap : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Vector2 minXY;
    [SerializeField] private Vector2 maxXY;

    public static Worldmap Instance {get; private set;}

    private void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(gameObject); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        if(mousePosition.x < maxXY.x && mousePosition.y < maxXY.y && mousePosition.x > minXY.x && mousePosition.y > minXY.y)
            OnWorldMapClicked?.Invoke(mousePosition);

        Debug.Log(mousePosition);
    }

    public delegate void OnWorldMapClickedDelegate(Vector2 mousePosition);
    public event OnWorldMapClickedDelegate OnWorldMapClicked;
}
