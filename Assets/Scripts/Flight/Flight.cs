using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Flight
{
    [Header("General")]
    public int flightID;

    [Header("Plane")]
    [HideInInspector] public Airplane planeInstance;
    public float flightSpeed;

    [Header("Locations")]
    public LocationInfo departureLocation;
    public LocationInfo destinationLocation;
    [HideInInspector] public FlightLegIndicator departureIndicatorInstance;
    [HideInInspector] public FlightLegIndicator destinationIndicatorInstance;
    [HideInInspector] public FlightUI flightCardUIInstance;

    public Flight(LocationInfo _departureLocation, LocationInfo _destinationLocation, float _flightSpeed = 3f, int _flightID = 0)
    {
        flightID = _flightID;
        departureLocation = _departureLocation;
        destinationLocation = _destinationLocation;
        flightSpeed = _flightSpeed;
    }

    public IEnumerator StallFlight(float stallTime = 2f)
    {
        //Vector2 destination = planeInstance.destination;
        planeInstance.StopFlying();
        OnFlightStopped?.Invoke();
        yield return new WaitForSeconds(stallTime);
        planeInstance.FlyToDestination();
        OnFlightResumed?.Invoke();
    }

    public delegate void OnFlightStoppedDelegate();
    public event OnFlightStoppedDelegate OnFlightStopped;

    public delegate void OnFlightResumedDelegate();
    public event OnFlightResumedDelegate OnFlightResumed;
}
