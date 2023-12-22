using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public int flightsStarted = 0;
    private float GracePeriod = 2f;

    public FlightLegIndicator flightLegIndicatorPrefab;
    public Airplane planePrefab;
    public FlightUI flightUIPrefab;
    public Level currentLevel;

    public Canvas gameOverCanvas;
    public Canvas pauseCanvas;
    public Canvas winCanvas;

    public TextMeshProUGUI timerText;
    public float timeInLevel {get; private set;}

    public MapLocationDictionary mapLocationDictionary;
    public static GameManager Instance {get; private set;}

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

        flightsStarted = 0;
        pauseCanvas.enabled = false;
        gameOverCanvas.enabled = false;
        winCanvas.enabled = false;
        StartLevel();

        timerText.text = "09:00";
    }

    public void StartLevel()
    {
        StartCoroutine(PlayLevelCo());
        StartCoroutine(TrackGameTime());
    }

    private IEnumerator TrackGameTime()
    {
        while(true)
        {
            yield return null;
            timeInLevel += Time.deltaTime;
            UpdateTimerText();
        }
    }

    // public void PauseLevel()
    // {

    // }

    // public void ResumeLevel()
    // {

    // }

    public void EndLevel()
    {
        winCanvas.enabled = true;
        Time.timeScale = 0;
        foreach(LevelEvent levelEvent in currentLevel.events)
        {
            levelEvent.triggered = false;
        }
    }

    private IEnumerator PlayLevelCo()
    {
        if(currentLevel.events != null)
        {
            foreach(LevelEvent levelEvent in currentLevel.events)
            {
                levelEvent.triggered = false;
            }
        }

        timeInLevel = 0;
        CheckForTimeTriggeredEvents();
        yield return new WaitForSeconds(GracePeriod);
        timeInLevel = GracePeriod;
        FlightManager.Instance.OnPlaneCrash += DisplayGameOver;
        while(timeInLevel < currentLevel.levelTime)
        {
            int newPlanes;

            if(FlightManager.Instance.flightsInAir.Count < currentLevel.maxFlightsAtOneTime)
            {
                if(currentLevel.levelDifficulty <= 4) newPlanes = Random.Range(1,3);
                else if(currentLevel.levelDifficulty <= 8) newPlanes = Random.Range(2,4);
                else if(currentLevel.levelDifficulty <= 12) newPlanes = Random.Range(3,5);
                else newPlanes = Random.Range(4,6);

                for(int i = 0; i < newPlanes; i++)
                {
                    Flight newFlight = new Flight(GetRandomLocation(), GetRandomLocation());
                    int locationCheckLimit = 100;

                    while(!FlightManager.Instance.CanPlaceFlight(newFlight) && locationCheckLimit > 0)
                    {
                        newFlight.departureLocation = GetRandomLocation();
                        locationCheckLimit--;
                    }

                    if(FlightManager.Instance.StartFlight(newFlight)) flightsStarted++;
                    yield return new WaitForSeconds(.5f);
                }
            }

            float timeBeforeNextFlight = Random.Range(currentLevel.minTimeBetweenFlights, currentLevel.maxTimeBetweenFlights);
            yield return new WaitForSeconds(timeBeforeNextFlight);
        }

        EndLevel();
        StopCoroutine(PlayLevelCo());
    }

    private void CheckForTimeTriggeredEvents()
    {
        if(currentLevel.events != null)
        {
            foreach(LevelEvent levelEvent in currentLevel.events)
            {
                if(!levelEvent.triggered && levelEvent.timeOfEvent >= 0 && levelEvent.timeOfEvent <= timeInLevel)
                {
                    GameEventManager.Instance.DisplayGameEvent(levelEvent.gameEvent);
                    break;
                }
            }
        }
    }

    private void UpdateTimerText()
    {
        int hour = 9 + 8 * (int) timeInLevel / (int) currentLevel.levelTime;
        int minutes = (480 * (int) timeInLevel / (int) currentLevel.levelTime) % 60;

        string _hour;
        if(hour < 10)
            _hour = "0" + hour.ToString();
        else _hour = hour.ToString();

        string _minutes;
        if(minutes < 10)
            _minutes = "0" + minutes.ToString();
        else _minutes = minutes.ToString();

        timerText.text = $"{_hour}:{_minutes}";
    }

    private LocationInfo GetRandomLocation()
    {
        return MapLocationDictionary.Instance.locations[Random.Range(0, MapLocationDictionary.Instance.locations.Count)];
    }

    private void DisplayGameOver()
    {
        gameOverCanvas.enabled = true;
        Time.timeScale = 0;
    }
}