using UnityEngine;

public class BossEnemyStateAttack : BossEnemyStateBase
{
    private void Update()
    {
        bossEnemy.dis = Vector3.Distance(bossEnemy.player.transform.position, bossEnemy.transform.position);
        bossEnemy.dir = (bossEnemy.player.transform.position - bossEnemy.transform.position).normalized;
    }
}
