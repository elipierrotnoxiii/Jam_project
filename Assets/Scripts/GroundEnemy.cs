using System;
using UnityEngine;

public class GroundEnemy : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 5f;
    public float speed = 2f;
    public float patrolRadius = 3f;
    public float patrolSpeed = 2f;

    private Vector3 startPosition;
    private float patrolAngle = 0f;
    private bool isChasing = false;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            isChasing = true;
        }
        else
        {
            isChasing = false;
        }

        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        patrolAngle += patrolSpeed * Time.deltaTime;
        float x = Mathf.Cos(patrolAngle) * patrolRadius;
        float z = Mathf.Sin(patrolAngle) * patrolRadius;

        Vector3 patrolPosition = startPosition + new Vector3(x, 0, z);

        transform.position = Vector3.MoveTowards(transform.position, patrolPosition, speed * Time.deltaTime);


    }

    void ChasePlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

        Vector3 dir = (player.position - transform.position).normalized;
        if (dir != Vector3.zero)
            transform.forward = dir;
    }
}
