using System.Collections;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    public Animator animator;
    public Transform hitboxOrigin;

    public Transform hitboxStunOrigin;
    public float radius = 0.5f;

    public float radiusStun = 20f;
    public LayerMask enemyLayer;
    
    private bool isAttacking;

    private bool isCooldown;

    public SkillCooldownUI skillCooldown;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            StartCoroutine(Attack());
        }

        if (Input.GetKeyDown(KeyCode.E) && !isCooldown)
        {
            StartCoroutine(StunFlyingEnemies());
        }
    }



    private IEnumerator StunFlyingEnemies()
    {
        isCooldown = true;

        print("Skill Cooldown");
        yield return new WaitForSeconds(0.2f);

        skillCooldown.UseSkill();

        Collider[] hitEnemies = Physics.OverlapSphere(hitboxStunOrigin.position, radiusStun, enemyLayer);
        foreach (Collider enemy in hitEnemies)
        {
            FlyingEnemy flying = enemy.GetComponent<FlyingEnemy>();
            if (flying != null)
            {
                flying.Stun(3f); // Assumes FlyingEnemy has a Stun(float duration) method
            }
        }

        yield return new WaitForSeconds(5f);
        isCooldown = false;
        print("Skill Ready");
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
            EnemyHealth eHealth = enemy.GetComponent<EnemyHealth>();
            if (eHealth != null)
            {
                eHealth.TakeDamage(1); // inflige 1 de daño
            }
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

        if (hitboxStunOrigin != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(hitboxStunOrigin.position, radiusStun);
        }
    }    
}
