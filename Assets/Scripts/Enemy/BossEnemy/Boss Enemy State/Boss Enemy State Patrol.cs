using UnityEngine;

public class BossEnemyStatePatrol : BossEnemyStateBase
{
    private void Update()
    {
        Patrol();
    }
    private void Patrol()
    {
        bossEnemy.transform.position += (Vector3)bossEnemy.patrolDir * bossEnemy.moveSpeed * Time.deltaTime;
        RaycastHit2D hit = Physics2D.Raycast(bossEnemy.transform.position, bossEnemy.patrolDir, bossEnemy.obstacleDetectDistance, bossEnemy.obstacleLayer);
        if (hit.collider != null)
        {
            bossEnemy.patrolDir *= -1;
        }

        Collider2D player = Physics2D.OverlapCircle(bossEnemy.transform.position, bossEnemy.playerDetectRange, bossEnemy.playerLayer);
        if (player != null)
        {
            bossEnemy.player = player.GetComponent<PlayerBase>();
        }
    }
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(bossEnemy.transform.position, bossEnemy.playerDetectRange);

    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawLine(bossEnemy.transform.position, (Vector2)bossEnemy.transform.position + bossEnemy.patrolDir * bossEnemy.obstacleDetectDistance);
    //}
}
