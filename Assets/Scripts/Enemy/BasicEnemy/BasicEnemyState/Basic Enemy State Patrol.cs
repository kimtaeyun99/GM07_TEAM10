using UnityEngine;

public class BasicEnemyStatePatrol : BasicEnemyStateBase
{
    private void Update()
    {
        Patrol();
    }
    private void Patrol()
    {
        basicEnemy.transform.position += (Vector3)basicEnemy.patrolDir * basicEnemy.moveSpeed * Time.deltaTime;
        RaycastHit2D hit = Physics2D.Raycast(basicEnemy.transform.position, basicEnemy.patrolDir, basicEnemy.obstacleDetectDistance, basicEnemy.obstacleLayer);
        if (hit.collider != null)
        {
            basicEnemy.patrolDir *= -1;
            basicEnemy.transform.Rotate(0, 180, 0);
        }

        Collider2D player = Physics2D.OverlapCircle(basicEnemy.transform.position, basicEnemy.playerDetectRange, basicEnemy.playerLayer);
        if (player != null)
        {
            basicEnemy.player = player.GetComponent<PlayerBase>();
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(basicEnemy.transform.position, basicEnemy.playerDetectRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(basicEnemy.transform.position, (Vector2)basicEnemy.transform.position + basicEnemy.patrolDir * basicEnemy.obstacleDetectDistance);
    }
}
