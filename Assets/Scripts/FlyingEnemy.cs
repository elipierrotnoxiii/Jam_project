using System.Collections;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 5f;
    public float speed = 3f;
    public float patrolRadius = 4f;
    public float patrolSpeed = 2f;
    public float fleeDistance = 5f;
    public float fleeDuration = 1f;

    Vector3 startPosition;
    float patrolAngle = 0f;
    bool isFleeing = false;
    bool canPatrol = true;
    bool isStunned = false; // NUEVO

    [Header("Particle")]
    public GameObject headBangPrefab; // NUEVO

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // CORREGIDO
        startPosition = transform.position;
        rb.useGravity = false; // Asegura que normalmente no tenga gravedad
        rb.linearVelocity = Vector3.zero;
    }

    void Update()
    {
        if (isStunned) return; // Si está aturdido, no hace nada

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange && !isFleeing)
        {
            StartCoroutine(FleeFromPlayer());
        }

        if (!isFleeing && canPatrol)
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

        Vector3 dir = (patrolPosition - transform.position).normalized;
        if (dir != Vector3.zero)
            transform.forward = dir;
    }

    public void Stun(float duration)
    {
        if (!isStunned)
        {
            StartCoroutine(StunCoroutine(duration));
        }
    }

    private IEnumerator StunCoroutine(float duration) // colocar aqui la particula de estrellita
    {

        headBangPrefab.SetActive(true);
        isStunned = true;
        canPatrol = false;
        isFleeing = false;

        rb.useGravity = true;
        rb.linearVelocity = Vector3.zero; // Detiene cualquier movimiento
        // Opcional: puedes desactivar el collider si quieres que no interactúe con nada más

        yield return new WaitForSeconds(duration);

        rb.useGravity = false;
        rb.linearVelocity = Vector3.zero;
        isStunned = false;
        canPatrol = true;
        headBangPrefab.SetActive(false);

        // El enemigo vuelve a volar y patrullar
    }

    IEnumerator FleeFromPlayer()
    {
        isFleeing = true;
        canPatrol = false;

        Vector3 fleeDirection = (transform.position - player.position).normalized;
        Vector3 targetPosition = transform.position + fleeDirection * fleeDistance;

        float elapsed = 0f;
        while (elapsed < fleeDuration && !isStunned) // Solo huye si no está aturdido
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            Vector3 dir = (targetPosition - transform.position).normalized;
            if (dir != Vector3.zero)
                transform.forward = dir;

            elapsed += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        isFleeing = false;
        canPatrol = true;
    }
}
