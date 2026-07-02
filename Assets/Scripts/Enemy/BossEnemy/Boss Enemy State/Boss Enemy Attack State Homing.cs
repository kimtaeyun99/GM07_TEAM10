using System.Collections;
using UnityEngine;

public class BossEnemyAttackStateHoming : BossEnemyStateAttack
{
    private Vector3 attackPosition;
    private void OnEnable()
    {
        attackPosition = bossEnemy.transform.position;
        StartCoroutine(HomingCo());
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    private void Update()
    {
        bossEnemy.transform.position = attackPosition;
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
            Managers.EnemyAudio.BossEnemyHomingAttack();
            yield return bossEnemy.HomingAttackWait;
        }
        bossEnemy.attackTimer = 0f;
        bossEnemyStateManager.SetAttackState(BossEnemyStateManager.BossEnemyAttackState.None);
        bossEnemyStateManager.SetState(BossEnemyStateManager.BossEnemyState.Chase);

        yield break;
    }
}
