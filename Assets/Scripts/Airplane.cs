using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Airplane : MonoBehaviour
{
    [HideInInspector] public int flightID;
    [SerializeField] private Rigidbody2D rb2d;
    [SerializeField] private Collider2D planeCollider;
    //[HideInInspector] public Vector2 destination;
    private List<Vector2> destinations;
    public float flightSpeed = 1f;

    [SerializeField] private Image planeSprite;
    [SerializeField] private Sprite planeUp;
    [SerializeField] private Sprite planeDown;
    [SerializeField] private Sprite planeRight;
    [SerializeField] private Sprite planeLeft;
    private List<Sprite> planeSprites;

    private Coroutine spinCoroutine;

    private void Awake()
    {
        planeSprites = new List<Sprite>(){planeUp, planeRight, planeDown, planeLeft};
        spinCoroutine = null;
    }

    private void Update()
    {
        if(rb2d.velocity.magnitude > 0 && Vector2.Distance(transform.position, destinations[0]) < .01f)
        {
            destinations.RemoveAt(0);
            FlyToDestination();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Hazard"))
            FlightManager.Instance.CrashPlanes(new List<Airplane>() {this});
        if(other.gameObject.TryGetComponent<Airplane>(out Airplane airplane) && airplane != null)
            FlightManager.Instance.CrashPlanes(new List<Airplane>() {this, airplane});
    }

    public void FlyToDestination(Vector2 destination, bool hasPriority = true, bool stopSpin = true)
    {
        if(destinations == null) destinations = new List<Vector2>();

        if(stopSpin && spinCoroutine != null)
        {
            StopCoroutine(spinCoroutine);
            spinCoroutine = null;
        }

        if(hasPriority) destinations.Insert(0, destination);
        else destinations.Add(destination);

        planeCollider.enabled = true;
        //this.destination = destinations[0];
        rb2d.velocity = Vector2.ClampMagnitude(destinations[0] - (Vector2) transform.position, 1) * flightSpeed;
        
        if(Math.Abs(rb2d.velocity.x) > Math.Abs(rb2d.velocity.y))
        {
            if(rb2d.velocity.x > 0) planeSprite.sprite = planeRight;
            else planeSprite.sprite = planeLeft;
        }
        else
        {
            if(rb2d.velocity.y > 0) planeSprite.sprite = planeUp;
            else planeSprite.sprite = planeDown;
        }
    }

    public void FlyToDestination(bool stopSpin = true)
    {
        if(stopSpin && spinCoroutine != null)
        {
            StopCoroutine(spinCoroutine);
            spinCoroutine = null;
        }

        planeCollider.enabled = true;
        rb2d.velocity = Vector2.ClampMagnitude(destinations[0] - (Vector2) transform.position, 1) * flightSpeed;
        
        if(Math.Abs(rb2d.velocity.x) > Math.Abs(rb2d.velocity.y))
        {
            if(rb2d.velocity.x > 0) planeSprite.sprite = planeRight;
            else planeSprite.sprite = planeLeft;
        }
        else
        {
            if(rb2d.velocity.y > 0) planeSprite.sprite = planeUp;
            else planeSprite.sprite = planeDown;
        }
    }

    public void StopFlying(bool spin = true)
    {
        if(spin && spinCoroutine == null)
            spinCoroutine = StartCoroutine(Spin());

        //destination = Vector2.zero;
        planeCollider.enabled = false;
        rb2d.velocity = Vector2.zero;
    }

    private IEnumerator Spin()
    {
        int spinValue = 0;
        while(true)
        {
            yield return new WaitForSeconds(.25f);
            planeSprite.sprite = planeSprites[spinValue % planeSprites.Count];
            spinValue++;
        }
    }

    public void Takeoff()
    {
        OnTakeoff?.Invoke();
    }

    public delegate void OnTakeoffDelegate();
    public event OnTakeoffDelegate OnTakeoff;

    public void Reroute(Vector2 rerouteDestination)
    {
        FlyToDestination(rerouteDestination);
        Worldmap.Instance.OnWorldMapClicked -= Reroute;
    }
}