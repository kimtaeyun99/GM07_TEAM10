using UnityEngine;

public class EnemyBulletManager : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private EnemyBullet enemyBulletPrefab;
    public void FireCirclePattern(int bulletCount)
    {
        float angleStep = 360f / bulletCount;
        float angle = 0f;

        for (int i = 0; i < bulletCount; i++)
        {
            float dirX = Mathf.Cos(angle * Mathf.Deg2Rad);
            float dirY = Mathf.Sin(angle * Mathf.Deg2Rad);
            Vector3 dir = new Vector3(dirX, dirY, 0f);

            EnemyBullet bullet = Instantiate(enemyBulletPrefab, firePoint.position, Quaternion.identity);
            bullet.Initialize(dir, EnemyBullet.BulletPattern.Straight);

            angle += angleStep;
        }
    }
    public void FireSpiralPattern(int bulletCount, float angleOffset)
    {
        float angleStep = 360f / bulletCount;
        float angle = angleOffset;

        for (int i = 0; i < bulletCount; i++)
        {
            float dirX = Mathf.Cos(angle * Mathf.Deg2Rad);
            float dirY = Mathf.Sin(angle * Mathf.Deg2Rad);
            Vector3 dir = new Vector3(dirX, dirY, 0f);

            EnemyBullet bullet = Instantiate(enemyBulletPrefab, firePoint.position, Quaternion.identity);
            bullet.Initialize(dir, EnemyBullet.BulletPattern.Spiral);

            angle += angleStep;
        }
    }
}
