using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;

    [SerializeField] private float spawnRadius = 5;

    private bool isSpawnable = true;

    private void Start()
    {
        SpawnEnemy(enemyData);
    }

    private void SpawnEnemy(EnemyData enemyData)
    {
        if (!isSpawnable) return;
        if (enemyData == null) return;
        if (enemyData.EnemyPrefab == null) return;

        Vector2 spawnPos = Random.insideUnitCircle * spawnRadius;

        EnemyBase enemy = Instantiate(enemyData.EnemyPrefab, spawnPos, Quaternion.identity);

        enemy.Initialize(enemyData);
        isSpawnable = false;
    }
}
