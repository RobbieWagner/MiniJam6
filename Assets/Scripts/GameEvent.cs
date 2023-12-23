using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

[CreateAssetMenu(menuName = "Flights/GameEvent")]
public class GameEvent : ScriptableObject
{
    [Header("Alert")]
    public Sprite alertIcon;
    public string alertTitle;
    [TextArea(5,20)] public string[] alertBody;
    public List<VisualAid> visualAids;
    public Vector2 windowSize;

    [Header("Event Effects")]
    public EventSpawn[] eventSpawns;
}
