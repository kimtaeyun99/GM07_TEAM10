using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyData EnemyData;

    [SerializeField] private EnemyBase Enemyprefab;

    [SerializeField] private float spawnRadius = 5;
    [SerializeField] private int EnemySpawnCount = 3;
    private void Start()
    {
        SpawnEnemy(EnemyData);
    }

    private void SpawnEnemy(EnemyData enemyData)
    {
        for (int i = 0; i < EnemySpawnCount; i++)
        {
            if (enemyData == null) return;
            if (enemyData.EnemyPrefab == null) return;

            EnemyBase enemy = Managers.Pool.GetPool(Enemyprefab);
            enemy.transform.position = transform.position;
            enemy.transform.rotation = Quaternion.Euler(Vector2.right);
            enemy.Initialize(enemyData);
        }
    }
}
