using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelEvent
{
    public float timeOfEvent = -1f;
    public GameEvent gameEvent;
    [HideInInspector] public bool triggered;
}

[CreateAssetMenu(menuName = "Flights/Level")]
public class Level : ScriptableObject
{
    public int levelDifficulty = 3;
    public int flightCount = 25;
    public float levelTime = 180f;
    public int maxFlightsAtOneTime = 10;
    public float minTimeBetweenFlights = 3f;
    public float maxTimeBetweenFlights = 10f;
    public List<LevelEvent> events;
}