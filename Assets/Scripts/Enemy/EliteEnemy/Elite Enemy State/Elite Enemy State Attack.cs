using System.Collections;
using UnityEngine;

public class EliteEnemyStateAttack : EliteEnemyStateBase
{
    private Vector3 attackPosition;
    private void OnEnable()
    {
        attackPosition = eliteEnemy.transform.position;
        StartCoroutine(AttackCo());
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    private void Update()
    {
        eliteEnemy.transform.position = attackPosition;
        eliteEnemy.dis = Vector3.Distance(eliteEnemy.player.transform.position, eliteEnemy.transform.position);
        eliteEnemy.dir = (eliteEnemy.player.transform.position - eliteEnemy.transform.position).normalized;
    }
    private IEnumerator AttackCo()
    {
         int pattern = Random.Range(0, 5);
         switch (pattern)
         {
             case 0: yield return StartCoroutine(StraightAttackCo()); break;
             case 1: yield return StartCoroutine(CurveAttackCo()); break;
             case 2: yield return StartCoroutine(CircleAttackCo()); break;
             case 3: yield return StartCoroutine(SpiralAttackCo()); break;
             case 4: yield return StartCoroutine(HomingAttackCo()); break;
         }
    }
    private IEnumerator StraightAttackCo()
    {
        for (int i = 0; i < eliteEnemy.StraightAttackCount; i++)
        {
            EnemyBullet bullet = Managers.Pool.GetPool(eliteEnemy.EnemyBulletPrefab);
            bullet.transform.position = eliteEnemy.FirePoint.position;
            bullet.transform.rotation = Quaternion.FromToRotation(Vector3.right, eliteEnemy.dir);
            bullet.damage = eliteEnemy.attack;
            bullet.Initialize(eliteEnemy.dir, EnemyBullet.BulletPattern.Straight, eliteEnemy.player);
            Managers.EnemyAudio.EliteEnemyAttack();
            yield return eliteEnemy.StraightAttackWait;
        }
        eliteEnemy.attackTimer = 0f;
        yield break;
    }
    private IEnumerator CurveAttackCo()
    {
        for (int i = 0; i < eliteEnemy.CurveAttackRepeatCount; i++)
        {
            for (int a = 0; a < eliteEnemy.CurveAttackCount; a++)
            {
                EnemyBullet bullet = Managers.Pool.GetPool(eliteEnemy.EnemyBulletPrefab);
                bullet.transform.position = eliteEnemy.FirePoint.position;
                bullet.transform.rotation = Quaternion.FromToRotation(Vector3.right, eliteEnemy.dir);
                bullet.damage = eliteEnemy.attack;
                bullet.Initialize(eliteEnemy.dir, EnemyBullet.BulletPattern.Curve, eliteEnemy.player);
                Managers.EnemyAudio.EliteEnemyAttack();
                yield return eliteEnemy.CurveAttackWait;
            }
            yield return eliteEnemy.CurveAttackRepeatWait;
        }
        eliteEnemy.attackTimer = 0f;
        yield break;
    }
    private IEnumerator CircleAttackCo()
    {
        float angleStep = 360f / eliteEnemy.CircleAttackCount;
        float angle = 0f;

        Managers.EnemyAudio.EliteEnemyAttack();
        for (int i = 0; i < eliteEnemy.CircleAttackCount; i++)
        {
            float dirX = Mathf.Cos(angle * Mathf.Deg2Rad);
            float dirY = Mathf.Sin(angle * Mathf.Deg2Rad);
            Vector3 dir = new Vector3(dirX, dirY, 0f);

            EnemyBullet bullet = Managers.Pool.GetPool(eliteEnemy.EnemyBulletPrefab);
            bullet.transform.position = eliteEnemy.FirePoint.position;
            bullet.transform.rotation = Quaternion.FromToRotation(Vector3.right, dir);
            bullet.damage = eliteEnemy.attack;
            bullet.Initialize(dir, EnemyBullet.BulletPattern.Straight, eliteEnemy.player);
            angle += angleStep;
        }
        eliteEnemy.attackTimer = 0f;
        yield break;
    }
    private IEnumerator SpiralAttackCo()
    {
        float angle = 0f;
        for (int i = 0; i < eliteEnemy.SpiralAttackCount; i++)
        {
            float dirX = Mathf.Cos(angle * Mathf.Deg2Rad);
            float dirY = Mathf.Sin(angle * Mathf.Deg2Rad);
            Vector3 dir = new Vector3(dirX, dirY, 0f);

            EnemyBullet bullet = Managers.Pool.GetPool(eliteEnemy.EnemyBulletPrefab);
            bullet.transform.position = eliteEnemy.FirePoint.position;
            bullet.transform.rotation = Quaternion.FromToRotation(Vector3.right, dir);
            bullet.damage = eliteEnemy.attack;
            bullet.Initialize(dir, EnemyBullet.BulletPattern.Straight, null);
            Managers.EnemyAudio.EliteEnemyAttack();
            angle += eliteEnemy.SpiralAngle;
            yield return new WaitForSeconds(0.1f);
        }
        eliteEnemy.attackTimer = 0f;
    }
    private IEnumerator HomingAttackCo()
    {
        for (int i = 0; i < eliteEnemy.HomingAttackCount; i++)
        {
            EnemyBullet bullet = Managers.Pool.GetPool(eliteEnemy.EnemyBulletPrefab);
            bullet.transform.position = eliteEnemy.FirePoint.position;
            bullet.transform.rotation = Quaternion.FromToRotation(Vector3.right, eliteEnemy.dir);
            bullet.damage = eliteEnemy.attack;
            bullet.Initialize(eliteEnemy.dir, EnemyBullet.BulletPattern.Homing, eliteEnemy.player);
            Managers.EnemyAudio.EliteEnemyAttack();
            yield return eliteEnemy.HomingAttackWait;
        }
        eliteEnemy.attackTimer = 0f;
        yield break;
    }
}
