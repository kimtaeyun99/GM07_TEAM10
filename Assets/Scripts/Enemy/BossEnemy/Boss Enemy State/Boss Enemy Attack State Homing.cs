using System.Collections;
using UnityEngine;

public class BossEnemyAttackStateHoming : BossEnemyStateAttack
{
    private void OnEnable()
    {
        StartCoroutine(HomingCo());
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    private void Update()
    {
        bossEnemy.dis = Vector3.Distance(bossEnemy.player.transform.position, bossEnemy.transform.position);
        bossEnemy.dir = (bossEnemy.player.transform.position - bossEnemy.transform.position).normalized;
    }
    private IEnumerator HomingCo()
    {
        for (int i = 0; i < bossEnemy.HomingAttackCount; i++)
        {
            EnemyBullet bullet = Managers.Pool.GetPool(bossEnemy.EnemyBulletPrefab);
            bullet.transform.position = bossEnemy.FirePoint.position;
            bullet.transform.rotation = Quaternion.FromToRotation(Vector3.right, bossEnemy.dir);
            bullet.Initialize(bossEnemy.dir, EnemyBullet.BulletPattern.Homing, bossEnemy.player);
            yield return bossEnemy.HomingAttackWait;
        }
        bossEnemy.attackTimer = 0f;
        yield break;
    }
}
