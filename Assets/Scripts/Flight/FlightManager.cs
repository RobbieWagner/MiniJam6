using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlightManager : MonoBehaviour
{

    public Dictionary<Color, Flight> flightsInAir;
    [SerializeField] private int FlightLimit = 12;
    [SerializeField] private float TakeoffDelay = 2f;
    [SerializeField] private Canvas worldSpaceCanvas;
    [SerializeField] private Canvas planeCanvas;
    [SerializeField] private LayoutGroup flightDirectory;

    [SerializeField] private List<Color> planeColorOptions;

    private bool canCrashPlanes = true;

    HashSet<Color> currentUsedColors;

    public static FlightManager Instance {get; private set;}

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

        flightsInAir = new Dictionary<Color, Flight>();
        currentUsedColors = new HashSet<Color>();
    }

    public bool StartFlight(Flight flight)
    {
        Color flightColor = CreateUniqueColor();

        if(CanPlaceFlight(flight) && !flightsInAir.ContainsKey(flightColor) && flightsInAir.Count < FlightLimit)
        {
            flight.departureIndicatorInstance = Instantiate(GameManager.Instance.flightLegIndicatorPrefab, worldSpaceCanvas.transform);
            flight.destinationIndicatorInstance = Instantiate(GameManager.Instance.flightLegIndicatorPrefab, worldSpaceCanvas.transform);
            flight.planeInstance = Instantiate(GameManager.Instance.planePrefab, planeCanvas.transform).GetComponentInChildren<Airplane>();
            foreach(FlightUI controlCard in GameManager.Instance.controlCards)
            {
                if(!controlCard.gameObject.activeSelf)
                {
                    controlCard.gameObject.SetActive(true);
                    controlCard.flight = flight;
                    flight.flightCardUIInstance = controlCard;
                    break;
                }
            }
            flight.flightCardUIInstance.plane = flight.planeInstance;
            flight.flightCardUIInstance.flight = flight;
            flight.planeInstance.transform.parent.position = flight.departureLocation.coordinates;
            flight.departureIndicatorInstance.transform.position = flight.departureLocation.coordinates;
            flight.destinationIndicatorInstance.transform.position = flight.destinationLocation.coordinates;
            
            flight.planeInstance.transform.parent.GetComponent<Image>().color = flightColor;
            flight.flightCardUIInstance.UIColor = flightColor;
            flight.departureIndicatorInstance.blinkingLight.color = flightColor;
            flight.destinationIndicatorInstance.blinkingLight.color = flightColor;
            flight.destinationIndicatorInstance.isDestination = true;
            flight.destinationIndicatorInstance.flight = flight;

            flightsInAir.Add(flightColor, flight);
            StartCoroutine(StartFlightCo(flight));
            
            return true;
        }
        return false;
    }

    private Color CreateUniqueColor()
    {
        Color newColor = planeColorOptions[UnityEngine.Random.Range(0, planeColorOptions.Count)];

        while(currentUsedColors.Contains(newColor))
            newColor = planeColorOptions[UnityEngine.Random.Range(0, planeColorOptions.Count)];

        return newColor;
    }

    public void LandPlane(Flight flight)
    {
        if(flight != null)
        {
            Color color = flight.planeInstance.transform.parent.GetComponent<Image>().color;
            currentUsedColors.Remove(color);
            flightsInAir.Remove(color);

            OnLandPlane?.Invoke(flight);

            Destroy(flight.departureIndicatorInstance.gameObject);
            Destroy(flight.destinationIndicatorInstance.gameObject);
            Destroy(flight.planeInstance.transform.parent.gameObject);
            flight.flightCardUIInstance.flight = null;
            flight.flightCardUIInstance.gameObject.SetActive(false);
        }
    }

    public delegate void OnLandPlaneDelegate(Flight landedFlight);
    public event OnLandPlaneDelegate OnLandPlane;

    public void CrashPlanes(List<Airplane> planes)
    {
        if(canCrashPlanes)
        {
            canCrashPlanes = false;
            foreach(Airplane plane in planes)
            {
                foreach(Color flight in flightsInAir.Keys)
                {
                    if(plane.transform.parent.GetComponent<Image>().color == flight)
                    {
                        StartCoroutine(StopFlightCo(flight, flightsInAir[flight]));
                    }
                }
            }

            SoundManager.Instance.PlaySoundByName("Crash");
            OnPlaneCrash?.Invoke();
        }
    }

    public delegate void OnPlaneCrashDelegate();
    public event OnPlaneCrashDelegate OnPlaneCrash;

    public IEnumerator StopFlightCo(Color color, Flight flight)
    {
        Destroy(flight.destinationIndicatorInstance.gameObject);
        Destroy(flight.departureIndicatorInstance.gameObject);
        Destroy(flight.planeInstance.transform.parent.gameObject);
        flight.flightCardUIInstance.flight = null;
        flight.flightCardUIInstance.gameObject.SetActive(false);
     
        yield return null;

        flightsInAir.Remove(color);
        canCrashPlanes = true;
    }

    private IEnumerator StartFlightCo(Flight flight)
    {
        flight.planeInstance.StopFlying();
        yield return new WaitForSeconds(TakeoffDelay);
        flight.planeInstance.FlyToDestination(flight.destinationIndicatorInstance.transform.position);
        flight.planeInstance.Takeoff();
    }

    public void RerouteFlight(Airplane plane)
    {
        Worldmap.Instance.OnWorldMapClicked += plane.Reroute;
    }

    public bool CanPlaceFlight(Flight newFlight)
    {
        if(Vector2.Distance(newFlight.departureLocation.coordinates, newFlight.destinationLocation.coordinates) < 3) return false;

        foreach(Flight flight in flightsInAir.Values)
        {
            if(flight.departureLocation.coordinates == newFlight.departureLocation.coordinates) 
            {
                return false;
            }
            if(Vector2.Distance(flight.planeInstance.transform.position, newFlight.departureLocation.coordinates) < 3.5f)
             {
                return false;
            }
        }

        return true;
    }
}