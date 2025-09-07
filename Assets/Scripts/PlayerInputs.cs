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
    public PlayerMovement playerMovement;

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
        playerMovement.canMove = false;
        animator.SetBool("isSkill", true);
        print("Skill used");

        yield return new WaitForSeconds(0.2f);
        playerMovement.canMove = true;
        animator.SetBool("isSkill", false);
        skillCooldown.UseSkill();

        Collider[] hitEnemies = Physics.OverlapSphere(hitboxStunOrigin.position, radiusStun, enemyLayer);
        foreach (Collider enemy in hitEnemies)
        {
        // Intenta stunear cualquier enemigo que tenga el método Stun
        var ground = enemy.GetComponent<GroundEnemy>();
        if (ground != null)
            ground.Stun(3f);

        var flying = enemy.GetComponent<FlyingEnemy>();
        if (flying != null)
            flying.Stun(3f);

        var charger = enemy.GetComponent<ChargerEnemy>();
        if (charger != null)
            charger.Stun(3f);
        }

        yield return new WaitForSeconds(5f);
        isCooldown = false;
        print("Skill ready");
    }
    private IEnumerator Attack()
    {
        isAttacking = true;
        playerMovement.canMove = false;
        animator.SetBool("isAttacking", true);

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

        yield return new WaitForSeconds(0.5f);
        animator.SetBool("isAttacking", false);
        isAttacking = false;
        playerMovement.canMove = true;
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
