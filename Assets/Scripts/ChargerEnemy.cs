using System.Collections;
using UnityEngine;

public class ChargerEnemy : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 5f;
    public float patrolRadius = 3f;
    public float patrolSpeed = 2f;
    public float chargeForce = 500f;
    public float chargeCooldown = 2f;
    public float chargeDuration = 0.5f;
    public float chargeUpTime = 1f;

    private Vector3 startPosition;
    private float patrolAngle = 0f;
    private bool isCharging = false;
    private bool isOnCooldown = false;
    private bool isStunned = false;
    private Rigidbody rb;
    private Vector3 chargeDirection;

    void Start()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (isStunned || isCharging || isOnCooldown)
            return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            StartCoroutine(ChargeRoutine());
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
        transform.position = Vector3.MoveTowards(transform.position, patrolPosition, patrolSpeed * Time.deltaTime);
    }

    IEnumerator ChargeRoutine()
    {
        isCharging = true;
        rb.linearVelocity = Vector3.zero;
        yield return new WaitForSeconds(chargeUpTime);

        chargeDirection = (player.position - transform.position).normalized;
        rb.AddForce(chargeDirection * chargeForce, ForceMode.Impulse);

        yield return new WaitForSeconds(chargeDuration);

        rb.linearVelocity = Vector3.zero;
        isCharging = false;
        isOnCooldown = true;
        yield return new WaitForSeconds(chargeCooldown);
        isOnCooldown = false;
    }

    // --- STUN LOGIC ---
    public void Stun(float duration)
    {
        if (!isStunned)
        {
            StartCoroutine(StunCoroutine(duration));
        }
    }

    private IEnumerator StunCoroutine(float duration)
    {
        isStunned = true;
        rb.linearVelocity = Vector3.zero;
        yield return new WaitForSeconds(duration);
        isStunned = false;
    }
}
