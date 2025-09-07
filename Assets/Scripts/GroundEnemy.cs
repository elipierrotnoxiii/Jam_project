using System;
using UnityEngine;

public class GroundEnemy : MonoBehaviour
{
    public Transform player; // <-- Campo para asignar el player

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
        if (player != null)
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
                ChasePlayer(player.position);
            }
            else
            {
                Patrol();
            }
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

    void ChasePlayer(Vector3 playerPosition)
    {
        transform.position = Vector3.MoveTowards(transform.position, playerPosition, speed * Time.deltaTime);

        Vector3 dir = (playerPosition - transform.position).normalized;
        if (dir != Vector3.zero)
            transform.forward = dir;
    }
}
