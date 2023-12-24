using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    [SerializeField] private Vector2 pos1;
    [SerializeField] private Vector2 pos2;
    [SerializeField] private Vector2 initialPos;
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody2D rb2d;

    private void Awake()
    {
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        while(true)
        {
            rb2d.velocity = Vector2.ClampMagnitude(pos1 - (Vector2) transform.position, 1) * speed;
            while (Vector2.Distance(transform.position, pos1) > 1)
                yield return null;
            rb2d. velocity = Vector2.ClampMagnitude(pos2 - (Vector2) transform.position, 1) * speed;
            while (Vector2.Distance(transform.position, pos2) > 1)
                yield return null;
        }
    }
}
