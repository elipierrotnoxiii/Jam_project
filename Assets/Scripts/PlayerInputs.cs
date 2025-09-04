using System.Collections;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    public Animator animator;
    public Transform hitboxOrigin;
    public float radius = 0.5f;
    public LayerMask enemyLayer;

    private bool isAttacking;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        isAttacking = true;
        // animation start

        print("attack started");
        yield return new WaitForSeconds(0.2f);

        Collider[] hitEnemies = Physics.OverlapSphere(hitboxOrigin.position, radius, enemyLayer);
        foreach (Collider enemy in hitEnemies)
        {
            // Here we reduce enemy hit points; example: enemy.GetComponent<EnemyHealth>();
        }

        yield return new WaitForSeconds(0.3f); // animation end
        isAttacking = false;
        print("attack ended");
    }

    void OnDrawGizmos()
    {
        if (hitboxOrigin != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(hitboxOrigin.position, radius);
        }
    }
}
