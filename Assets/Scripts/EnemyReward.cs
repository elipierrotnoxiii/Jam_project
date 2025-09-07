using UnityEngine;

public class EnemyReward : MonoBehaviour
{
    public int points = 1;
    [HideInInspector] public EnemySpawner spawner;

    private EnemyHealth health;

    void Awake()
    {
        health = GetComponent<EnemyHealth>();
        if (health != null)
        {
            health.onDeath += OnDeath;
        }
    }

    void OnDeath()
    {
        if (spawner != null && health.spawnerData != null)
        {
            spawner.OnEnemyKilled(health.spawnerData);
        }
        GameManager.instance.AddScore(points);
    }
}
