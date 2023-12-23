using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CrashChecker : MonoBehaviour
{
    [SerializeField] private Airplane planeToIgnore;
    [SerializeField] private Image alertImage;

    private HashSet<Airplane> planesInRange;

    private void Awake()
    {
        alertImage.enabled = false;
        planesInRange = new HashSet<Airplane>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Airplane plane = other.GetComponent<Airplane>();

        if(plane != null && plane != planeToIgnore)
        {
            planesInRange.Add(plane);
            alertImage.enabled = true;
            SoundManager.Instance.PlaySoundByName("CrashSoon");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Airplane plane = other.GetComponent<Airplane>();
        planesInRange.Remove(plane);
        if(planesInRange.Count == 0)
        {
            alertImage.enabled = false;
        }
    }

}
