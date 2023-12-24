using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChemtrailPlane : Airplane
{
    private int regainOnStop = 0;

    protected override void Awake()
    {
        base.Awake();

        Worldmap.Instance.OnWorldMapRightClicked += MovePlane;
        OnReachDestination += IncreaseSatisfaction;
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        
    }

    private void MovePlane(Vector2 position)
    {
        destinations.Clear();
        FlyToDestination(position);

        regainOnStop = (int) Vector2.Distance(position, transform.position);
    }

    private void IncreaseSatisfaction()
    {
        GameManager.Instance.IncreaseCustomerSatisfaction(regainOnStop);
    }
}
