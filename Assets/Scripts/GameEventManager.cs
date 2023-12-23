using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System;

[System.Serializable]
public class EventSpawn
{
    public GameObject spawn;
    public Vector2 spawnLocation;
}

[System.Serializable]
public class VisualAid
{
    public Sprite visual;
    public int messageElement;
}

public class GameEventManager : MonoBehaviour
{
    private GameEvent currentGameEvent;

    public Canvas popupCanvas;
    public Popup popupPrefab;
    private Popup popupInstance;

    [SerializeField] private Canvas worldSpaceUI;

    private int currentPage;

    public static GameEventManager Instance {get; private set;}

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

    public void DisplayGameEvent(GameEvent gameEvent)
    {
        SoundManager.Instance.PlaySoundByName("Popup");
        Time.timeScale = 0;
        currentGameEvent = gameEvent;
        popupCanvas.enabled = true;
        popupInstance = Instantiate(popupPrefab, popupCanvas.transform);

        DisplayWindow(0);
    }

    public void DisplayWindow(int window)
    {
        currentPage = window;
        int pages = currentGameEvent.alertBody.Length;
        if(window > -1 && window < pages)
        {
            popupInstance.background.rectTransform.sizeDelta = currentGameEvent.windowSize;
            popupInstance.icon.sprite = currentGameEvent.alertIcon;
            popupInstance.titleText.text = currentGameEvent.alertTitle;
            popupInstance.bodyText.text = currentGameEvent.alertBody[window];

            if(window == pages - 1)
            {
                ToggleXOut(true);
                ToggleArrows(true, false);
            }
            else if(window == 0)
            {
                ToggleXOut(false);
                ToggleArrows(false, true);
            }
            else
            {
                ToggleXOut(false);
                ToggleArrows(true, true);
            }

            if(currentGameEvent.visualAids != null)
            {
                popupInstance.visualAid.enabled = false;
                foreach(VisualAid visualAid in currentGameEvent.visualAids)
                {
                    if(visualAid.messageElement == window)
                    {
                        popupInstance.visualAid.enabled = true;
                        popupInstance.visualAid.sprite = visualAid.visual;
                        //popupInstance.visualAid.SetNativeSize();
                        break;
                    }
                }
            }
        }
    }

    private void ToggleArrows(bool left, bool right)
    {

        popupInstance.leftArrow.OnLMBClicked -= PreviousPage;
        popupInstance.rightArrow.OnLMBClicked -= NextPage;

        if(left)
        {
            popupInstance.leftArrow.gameObject.SetActive(true);
            popupInstance.leftArrow.OnLMBClicked += PreviousPage;
        }
        else
            popupInstance.leftArrow.gameObject.SetActive(false);

        if(right)
        {
            popupInstance.rightArrow.gameObject.SetActive(true);
            popupInstance.rightArrow.OnLMBClicked += NextPage;
        }
        else
            popupInstance.rightArrow.gameObject.SetActive(false);
    }

    public void PreviousPage()
    {
        DisplayWindow(currentPage - 1);
    }

    public void NextPage()
    {
        DisplayWindow(currentPage + 1);
    }

    private void ToggleXOut(bool on)
    {
        if(on)
        {
            popupInstance.xOut.gameObject.SetActive(true);
            popupInstance.xOut.OnLMBClicked += CloseEventWindow;
        }
        else
        {
            popupInstance.xOut.OnLMBClicked -= CloseEventWindow;
            popupInstance.xOut.gameObject.SetActive(false);
        }  
    }

    public void CloseEventWindow()
    {
        Destroy(popupInstance.gameObject);
        popupInstance = null;
        popupCanvas.enabled = false;
        Time.timeScale = 1;

        if(currentGameEvent.eventSpawns?.Length > 0)
        {
            PlaceEventSpawns();
        }
    }

    public void PlaceEventSpawns()
    {
        foreach(EventSpawn spawn in currentGameEvent.eventSpawns)
        {
            GameObject newObj = Instantiate(spawn.spawn, worldSpaceUI.transform);
            newObj.transform.position = spawn.spawnLocation;
        }
    }

}
