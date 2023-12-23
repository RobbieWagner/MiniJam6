using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlightLegIndicator : MonoBehaviour
{

    [HideInInspector] public int flightID;
    [HideInInspector] public bool isDestination = false;
    public Image blinkingLight;
    public Flight flight = null;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.TryGetComponent<Airplane>(out Airplane airplane) && airplane != null && airplane.GetComponent<Image>().color.Equals(blinkingLight.color))
        {
            if(isDestination) 
            {
                FlightManager.Instance.LandPlane(flight);
            }
        }
    }
}
