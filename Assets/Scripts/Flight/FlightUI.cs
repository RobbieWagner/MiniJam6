using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class FlightUI : MonoBehaviour
{
    [HideInInspector] public Flight flight;
    private Airplane _plane;
    [HideInInspector] public Airplane plane
    {
        get {return _plane;}
        set
        {
            _plane = value;
            _plane.OnTakeoff += () => 
            {
                stallFlightButton.enabled = true;
                stallFlightButton.GetComponent<Image>().sprite = pauseSprite;
                rerouteButton.enabled = true;
                rerouteButton.GetComponent<Image>().sprite = rerouteSprite;
            };
        }
    }
    private float CooldownTime = 4f;

    public Button stallFlightButton;
    public Button rerouteButton;
    public Sprite blankButtonSprite;
    public Sprite pauseSprite;
    public Sprite rerouteSprite;

    public Image flightCardImage;
    public Image planeIcon;
    public Color UIColor
    {
        get{ return flightCardImage.color;}
        set
        {
            flightCardImage.color = value;
            planeIcon.color = value;
        }
    }

    [SerializeField] private FlightUIPlaceholder placeholder;

    private void Awake()
    {
        stallFlightButton.enabled = false;
        stallFlightButton.GetComponent<Image>().sprite = blankButtonSprite;
        rerouteButton.enabled = false;
        rerouteButton.GetComponent<Image>().sprite = blankButtonSprite;
    }

    public void StallFlightButton()
    {
        StartCoroutine(flight?.StallFlight());
        flight.OnFlightResumed += CooldownStall;
        stallFlightButton.enabled = false;
        stallFlightButton.GetComponent<Image>().sprite = blankButtonSprite;
    }

    public void CooldownStall()
    {
        StartCoroutine(CooldownButtonCo(stallFlightButton));
        flight.OnFlightResumed -= CooldownStall;
    }

    public void RerouteFlightButton()
    {
        FlightManager.Instance?.RerouteFlight(plane);
        rerouteButton.enabled = false;
        rerouteButton.GetComponent<Image>().sprite = blankButtonSprite;
    }

    private IEnumerator CooldownButtonCo(Button button)
    {
        yield return new WaitForSeconds(CooldownTime);
        stallFlightButton.GetComponent<Image>().sprite = pauseSprite;
        stallFlightButton.enabled = true;
        StopCoroutine(CooldownButtonCo(button));
    }
    
    //20w 13h
}
