using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 1;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Notificamos al GameManager que sume un punto
        GameManager.instance.AddScore(1);

        // Aquí puedes poner animación o partículas antes de destruir
        Destroy(gameObject);
    }
}
