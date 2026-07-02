using System.Collections;
using UnityEngine;

public class BasicEnemyStateAttack : BasicEnemyStateBase
{
    private Vector3 attackPosition;
    private void OnEnable()
    {
        attackPosition = basicEnemy.transform.position;
        StartCoroutine(AttackCo());
    }
    private void Update()
    {
        basicEnemy.transform.position = attackPosition;
        basicEnemy.dis = Vector3.Distance(basicEnemy.player.transform.position, basicEnemy.transform.position);
        basicEnemy.dir = (basicEnemy.player.transform.position - basicEnemy.transform.position).normalized;
    }
    private IEnumerator AttackCo()
    {
       for (int i = 0; i < basicEnemy.StraightAttackCount; i++)
       {   
            EnemyBullet bullet = Managers.Pool.GetPool(basicEnemy.EnemyBullet);
            bullet.transform.position = basicEnemy.FirePoint.position;
            bullet.transform.rotation = Quaternion.FromToRotation(Vector3.right, basicEnemy.dir);
            bullet.Initialize(basicEnemy.dir, EnemyBullet.BulletPattern.Straight, basicEnemy.player);
            bullet.damage = basicEnemy.attack;
            Managers.EnemyAudio.BasicEnemyAttack();
            yield return basicEnemy.StraightAttackWait;
       }
       basicEnemy.attackTimer = 0f;
    }
}
