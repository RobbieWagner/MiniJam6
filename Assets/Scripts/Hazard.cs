using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Hazard : MonoBehaviour
{
    [SerializeField] private float triggerWait = 3f;
    [SerializeField] private Image hazardDisplay;
    [SerializeField] private Collider2D coll;
    [SerializeField] private bool isConspiracy = false;

    private void Awake()
    {
        StartCoroutine(EnableTriggerCo());
        coll.enabled = false;
    }

    private IEnumerator EnableTriggerCo()
    {
        hazardDisplay.color = Color.clear;
        if(isConspiracy) yield return hazardDisplay.DOColor(new Color(1, 1f, .25f, .3f), triggerWait).SetEase(Ease.InSine).WaitForCompletion();
        else yield return hazardDisplay.DOColor(new Color(1, .25f, .25f, .3f), triggerWait).SetEase(Ease.InSine).WaitForCompletion();
        coll.enabled = true;
    }
}
