using UnityEngine;

public class BasicEnemyStateChase : BasicEnemyStateBase
{
    private void Update()
    {
        if(basicEnemy.dis > basicEnemy.distanceToPlayer)
        {
            Move();
        }
        else
        {
            Away();
        }
    }
    private void Move()
    {
        basicEnemy.transform.position += basicEnemy.dir * basicEnemy.moveSpeed * Time.deltaTime;
    }
    private void Away()
    {
        basicEnemy.transform.position -= basicEnemy.dir * basicEnemy.moveSpeed * Time.deltaTime;
    }
}
