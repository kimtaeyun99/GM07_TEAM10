using System.Collections;
using UnityEngine;

public class BasicEnemyStateAttack : BasicEnemyStateBase
{
    private void OnEnable()
    {
        StartCoroutine(AttackCo());
    }
    private IEnumerator AttackCo()
    {
       for (int i = 0; i < basicEnemy.StraightAttackCount; i++)
       {   
            EnemyBullet bullet = Managers.Pool.GetPool(basicEnemy.EnemyBullet);
            bullet.transform.position = basicEnemy.FirePoint.position;
            bullet.transform.rotation = Quaternion.FromToRotation(Vector3.right, basicEnemy.dir);
            bullet.Initialize(basicEnemy.dir, EnemyBullet.BulletPattern.Straight, basicEnemy.player);
            yield return basicEnemy.StraightAttackWait;
       }
       basicEnemy.attackTimer = 0f;
    }
}
