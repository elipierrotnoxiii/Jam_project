using UnityEngine;
using System.Collections;


public class ChargerEnemy : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 5f;
    public float patrolRadius = 3f;
    public float patrolSpeed = 2f;
    public float chargeForce = 500f;
    public float chargeCooldown = 2f;
    public float chargeDuration = 0.5f; // Tiempo durante el cual la fuerza se aplica
    public float chargeUpTime = 1f;     // Tiempo de "carga" antes de embestir

    private Vector3 startPosition;
    private float patrolAngle = 0f;
    private bool isCharging = false;
    private bool isOnCooldown = false;
    private Rigidbody rb;
    private Vector3 chargeDirection;

    void Start()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (isCharging || isOnCooldown)
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

        // 1. Carga (puedes poner animación aquí)
        yield return new WaitForSeconds(chargeUpTime);

        // 2. Calcula dirección y aplica fuerza
        chargeDirection = (player.position - transform.position).normalized;
        rb.AddForce(chargeDirection * chargeForce, ForceMode.Impulse);

        // 3. Espera a que termine la embestida
        yield return new WaitForSeconds(chargeDuration);

        rb.linearVelocity = Vector3.zero; // Detén el movimiento tras la embestida

        // 4. Cooldown antes de poder embestir de nuevo
        isCharging = false;
        isOnCooldown = true;
        yield return new WaitForSeconds(chargeCooldown);
        isOnCooldown = false;
    }
}
