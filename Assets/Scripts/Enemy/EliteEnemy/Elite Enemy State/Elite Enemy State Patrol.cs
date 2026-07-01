using UnityEngine;

public class EliteEnemyStatePatrol : EliteEnemyStateBase
{
    private void Update()
    {
        Patrol();
    }
    private void Patrol()
    {
        eliteEnemy.transform.position += (Vector3)eliteEnemy.patrolDir * eliteEnemy.moveSpeed * Time.deltaTime;
        RaycastHit2D hit = Physics2D.Raycast(eliteEnemy.transform.position, eliteEnemy.patrolDir, eliteEnemy.obstacleDetectDistance, eliteEnemy.obstacleLayer);
        if (hit.collider != null)
        {
            eliteEnemy.patrolDir *= -1;
        }

        Collider2D player = Physics2D.OverlapCircle(eliteEnemy.transform.position, eliteEnemy.playerDetectRange, eliteEnemy.playerLayer);
        if (player != null)
        {
            eliteEnemy.player = player.GetComponent<PlayerBase>();
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(eliteEnemy.transform.position, eliteEnemy.playerDetectRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(eliteEnemy.transform.position, (Vector2)eliteEnemy.transform.position + eliteEnemy.patrolDir * eliteEnemy.obstacleDetectDistance);
    }
}
