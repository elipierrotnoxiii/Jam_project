using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemySpawnData
    {
        public Transform spawnPoint;
        public GameObject enemyPrefab;
        [HideInInspector] public GameObject currentEnemy;
        [HideInInspector] public float respawnTimer;
    }

    public EnemySpawnData[] spawnPositions;

    [Header("Respawn Timing")]
    public float baseRespawnTime = 10f;
    public float minRespawnTime = 2f;
    public float timeToMinRespawn = 180f; // En 3 minutos llega al mínimo

    private float elapsedTime = 0f;

    public Transform player; // <-- Campo para asignar el player

    void Start()
    {
        foreach (var spawn in spawnPositions)
        {
            SpawnEnemy(spawn);
        }
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        float t = Mathf.Clamp01(elapsedTime / timeToMinRespawn);
        float currentRespawnTime = Mathf.Lerp(baseRespawnTime, minRespawnTime, t);

        foreach (var spawn in spawnPositions)
        {
            if (spawn.currentEnemy == null)
            {
                spawn.respawnTimer -= Time.deltaTime;
                if (spawn.respawnTimer <= 0f)
                {
                    SpawnEnemy(spawn, currentRespawnTime);
                }
            }
        }
    }

    void SpawnEnemy(EnemySpawnData spawn, float respawnTime = -1f)
    {
        spawn.currentEnemy = Instantiate(spawn.enemyPrefab, spawn.spawnPoint.position, spawn.spawnPoint.rotation);

        // Asigna el player a los enemigos instanciados
        var flying = spawn.currentEnemy.GetComponent<FlyingEnemy>();
        if (flying != null)
            flying.player = player;

        var ground = spawn.currentEnemy.GetComponent<GroundEnemy>();
        if (ground != null)
            ground.player = player;

        EnemyHealth health = spawn.currentEnemy.GetComponent<EnemyHealth>();
        if (health != null)
        {
            health.spawnerData = spawn;
        }
        EnemyReward reward = spawn.currentEnemy.GetComponent<EnemyReward>();
        if (reward != null)
        {
            reward.spawner = this;
        }

        spawn.respawnTimer = respawnTime > 0 ? respawnTime : baseRespawnTime;
    }

    // Llamado por EnemyReward al morir el enemigo
    public void OnEnemyKilled(EnemySpawnData spawn)
    {
        spawn.currentEnemy = null;
        // El respawnTimer ya se setea en Update
    }
}
