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
            flight.planeInstance = Instantiate(GameManager.Instance.planePrefab, planeCanvas.transform);
            flight.flightCardUIInstance = Instantiate(GameManager.Instance.flightUIPrefab, flightDirectory.transform);
            flight.flightCardUIInstance.plane = flight.planeInstance;
            flight.flightCardUIInstance.flight = flight;
            flight.planeInstance.transform.position = flight.departureLocation.coordinates;
            flight.departureIndicatorInstance.transform.position = flight.departureLocation.coordinates;
            flight.destinationIndicatorInstance.transform.position = flight.destinationLocation.coordinates;
            
            flight.planeInstance.GetComponent<Image>().color = flightColor;
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
            Color color = flight.planeInstance.GetComponent<Image>().color;
            currentUsedColors.Remove(color);
            flightsInAir.Remove(color);

            OnLandPlane?.Invoke(flight);

            Destroy(flight.departureIndicatorInstance.gameObject);
            Destroy(flight.destinationIndicatorInstance.gameObject);
            Destroy(flight.planeInstance.gameObject);
            Destroy(flight.flightCardUIInstance.gameObject);
        }
    }

    public delegate void OnLandPlaneDelegate(Flight landedFlight);
    public event OnLandPlaneDelegate OnLandPlane;

    public void CrashPlanes(List<Airplane> planes)
    {
        Debug.Log("hello");
        OnPlaneCrash?.Invoke();
    }

    public delegate void OnPlaneCrashDelegate();
    public event OnPlaneCrashDelegate OnPlaneCrash;

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
            if(flight.departureLocation.coordinates == newFlight.departureLocation.coordinates) return false;
            if(Vector2.Distance(flight.planeInstance.transform.position, flight.departureLocation.coordinates) < 3.5f) return false;
        }

        return true;
    }
}