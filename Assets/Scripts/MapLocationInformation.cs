using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LocationInfo
{
    public string name;
    public Vector2 coordinates;
    public bool isInWater;
}

[CreateAssetMenu(menuName = "Flights/MapLocationDictionary")]
public class MapLocationDictionary : ScriptableObject
{
    public List<LocationInfo> locations;
    public static MapLocationDictionary Instance => GameManager.Instance.mapLocationDictionary;
}
