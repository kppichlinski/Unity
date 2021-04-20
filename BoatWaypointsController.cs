using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatWaypointsController : MonoBehaviour
{
    public List<Transform> waypoints = new List<Transform>();
    private Transform targetWaypoint;
    private int targetWaypointIndex = 0;
    private float minDistance = 0.1f;
    private int lastWaypointIndex;
    private bool way = true;
    // true - forward, false - backward
    private bool isStopped = false;
    private bool canMove = true;
    public bool waitingON;
    private GameObject player;
    private Collider playerCollider;

    public float movementSpeed = 3.0f;
    public float rotationSpeed = 2.0f;

    private Quaternion rotationToTarget;
    private Quaternion startingRotation;

    private float movementStep;
    private float rotationStep;

    void Start()
    {
        startingRotation = transform.rotation;
        lastWaypointIndex = waypoints.Count - 1;
        targetWaypoint = waypoints[targetWaypointIndex];

        player = GameObject.FindGameObjectWithTag("Player");
        playerCollider = player.GetComponent<Collider>();
    }

    void Update()
    {
        movementStep = movementSpeed * Time.deltaTime;
        rotationStep = rotationSpeed * Time.deltaTime;

        if (canMove)
        {
            Vector3 directionToTarget = targetWaypoint.position - transform.position;
            rotationToTarget = Quaternion.LookRotation(directionToTarget);

            transform.rotation = Quaternion.Slerp(transform.rotation, rotationToTarget, rotationStep);

            float distance = Vector3.Distance(transform.position, targetWaypoint.position);
            ChceckDistanceToWayPoint(distance, rotationToTarget);

            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, movementStep);
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, startingRotation, rotationStep);
        }
    }

    void ChceckDistanceToWayPoint(float currentDistance, Quaternion rotationToTarget)
    {
        if (currentDistance <= minDistance)
        {
            if (way)
            {
                targetWaypointIndex++;

                if (targetWaypointIndex == 1 && !isStopped && waitingON)
                {
                    StartCoroutine(WaitForPlayer(3));

                    isStopped = true;
                }
            }
            else
            {
                targetWaypointIndex--;

                if (isStopped)
                {
                    isStopped = false;
                }
            }
            UpdateTargetWaypoint();
        }
    }

    void UpdateTargetWaypoint()
    {
        if (targetWaypointIndex >= lastWaypointIndex)
        {
            way = false;
        }
        else if (targetWaypointIndex <= 0)
        {
            way = true;
        }
        targetWaypoint = waypoints[targetWaypointIndex];
    }

    IEnumerator WaitForPlayer(float time)
    {
        canMove = false;

        yield return new WaitForSeconds(time);

        canMove = true;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col == playerCollider)
        {
            player.transform.SetParent(transform);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col == playerCollider)
        {
            player.transform.SetParent(null);
        }
    }
}

