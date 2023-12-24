using System.Collections;
using System.Collections.Generic;
using RobbieWagnerGames;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Worldmap : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Vector2 minXY;
    [SerializeField] private Vector2 maxXY;
    [SerializeField] private GameControls inputActions;

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

        inputActions = new GameControls();
        inputActions.Enable();
        inputActions.Map.RightClick.performed += RightClick;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        if(mousePosition.x < maxXY.x && mousePosition.y < maxXY.y && mousePosition.x > minXY.x && mousePosition.y > minXY.y)
            OnWorldMapClicked?.Invoke(mousePosition);
    }

    public delegate void OnWorldMapClickedDelegate(Vector2 mousePosition);
    public event OnWorldMapClickedDelegate OnWorldMapClicked;

    public void RightClick(InputAction.CallbackContext context)
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if(mousePosition.x < maxXY.x && mousePosition.y < maxXY.y && mousePosition.x > minXY.x && mousePosition.y > minXY.y)
            OnWorldMapRightClicked?.Invoke(mousePosition);
    }

    public delegate void OnWorldMapRightClickedDelegate(Vector2 mousePosition);
    public event OnWorldMapRightClickedDelegate OnWorldMapRightClicked;
}
