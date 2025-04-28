using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;

    private Vector3 startPosition;
    private Vector3 endPosition;
    private float journeyLength;
    private float startTime;

    void Start()
    {
        startPosition = pointA.position;
        endPosition = pointB.position;
        journeyLength = Vector3.Distance(startPosition, endPosition);
        startTime = Time.time;
    }

    void Update()
    {
        float distCovered = (Time.time - startTime) * speed;
        float fractionOfJourney = distCovered / journeyLength;
        fractionOfJourney = Mathf.SmoothStep(0f, 1f, fractionOfJourney); // Movimiento suave

        transform.position = Vector3.Lerp(startPosition, endPosition, fractionOfJourney);

        if (fractionOfJourney >= 1f)
        {
            FlipDirection();
        }
    }

    void FlipDirection()
    {
        Vector3 temp = startPosition;
        startPosition = endPosition;
        endPosition = temp;
        journeyLength = Vector3.Distance(startPosition, endPosition);
        startTime = Time.time;
    }
}
