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
    public float minRespawnTime = 3f; 

    public Transform player;

    void Start()
    {
        float currentRespawnTime = GetCurrentRespawnTime();
        foreach (var spawn in spawnPositions)
        {
            SpawnEnemy(spawn, currentRespawnTime);
        }
    }

    void Update()
    {
        float currentRespawnTime = GetCurrentRespawnTime();

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

    float GetCurrentRespawnTime()
    {
        // Usa timerRemaining como tiempo restante 
        float timerRemaining = GameManager.instance != null ? GameManager.instance.timerRemaining : 600f;
        float matchDuration = 600f; 

        // Calcula el progreso de la partida (0 al inicio, 1 al final)
        float t = 1f - Mathf.Clamp01(timerRemaining / matchDuration);

        // Interpola entre baseRespawnTime y minRespawnTime según el progreso
        float currentRespawnTime = Mathf.Lerp(baseRespawnTime, minRespawnTime, t);
        return currentRespawnTime;
    }

    void SpawnEnemy(EnemySpawnData spawn, float respawnTime = -1f)
    {
        spawn.currentEnemy = Instantiate(spawn.enemyPrefab, spawn.spawnPoint.position, spawn.spawnPoint.rotation);

        var flying = spawn.currentEnemy.GetComponent<FlyingEnemy>();
        if (flying != null)
            flying.player = player;

        var ground = spawn.currentEnemy.GetComponent<GroundEnemy>();
        if (ground != null)
            ground.player = player;

        var charger = spawn.currentEnemy.GetComponent<ChargerEnemy>();
        if (charger != null)
            charger.player = player;

        EnemyHealth health = spawn.currentEnemy.GetComponent<EnemyHealth>();
        if (health != null)
            health.spawnerData = spawn;

        EnemyReward reward = spawn.currentEnemy.GetComponent<EnemyReward>();
        if (reward != null)
            reward.spawner = this;

        spawn.respawnTimer = respawnTime > 0 ? respawnTime : baseRespawnTime;
    }

    public void OnEnemyKilled(EnemySpawnData spawn)
    {
        spawn.currentEnemy = null;
    }
}
