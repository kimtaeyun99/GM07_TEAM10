using System.Collections;
using UnityEngine;

public class BossEnemyAttackStateNonHoming : BossEnemyStateAttack
{
    private void OnEnable()
    {
        StartCoroutine(NonHomingCo());
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
    private IEnumerator NonHomingCo()
    {
        switch(bossEnemy.attackPattern)
        {
            case 0: yield return StartCoroutine(StraightAttackCo()); break;
            case 1: yield return StartCoroutine(CurveAttackCo()); break;
            case 2: yield return StartCoroutine(CircleAttackCo()); break;
            case 3: yield return StartCoroutine(SpiralAttackCo()); break;
        }
    }
    private IEnumerator StraightAttackCo()
    {
        for (int i = 0; i < bossEnemy.StraightAttackCount; i++)
        {
            EnemyBullet bullet = Managers.Pool.GetPool(bossEnemy.EnemyBulletPrefab);
            bullet.transform.position = bossEnemy.FirePoint.position;
            bullet.transform.rotation = Quaternion.FromToRotation(Vector3.right, bossEnemy.dir);
            bullet.Initialize(bossEnemy.dir, EnemyBullet.BulletPattern.Straight, bossEnemy.player);
            yield return bossEnemy.StraightAttackWait;
        }
        bossEnemy.attackTimer = 0f;
        yield break;
    }
    private IEnumerator CurveAttackCo()
    {
        for (int i = 0; i < bossEnemy.CurveAttackRepeatCount; i++)
        {
            for (int a = 0; a < bossEnemy.CurveAttackCount; a++)
            {
                EnemyBullet bullet = Managers.Pool.GetPool(bossEnemy.EnemyBulletPrefab);
                bullet.transform.position = bossEnemy.FirePoint.position;
                bullet.transform.rotation = Quaternion.FromToRotation(Vector3.right, bossEnemy.dir);
                bullet.Initialize(bossEnemy.dir, EnemyBullet.BulletPattern.Curve, bossEnemy.player);
                yield return bossEnemy.CurveAttackWait;
            }
            yield return bossEnemy.CurveAttackRepeatWait;
        }
        bossEnemy.attackTimer = 0f;
        yield break;
    }
    private IEnumerator CircleAttackCo()
    {
        float angleStep = 360f / bossEnemy.CircleAttackCount;
        float angle = 0f;
        for (int a = 0; a < bossEnemy.CircleAttackRepeatCount; a++)
        {
            if (a % 2 == 0)
            {
                angle += bossEnemy.CircleAttackAngleOffset;
            }
            for (int i = 0; i < bossEnemy.CircleAttackCount; i++)
            {
                float dirX = Mathf.Cos(angle * Mathf.Deg2Rad);
                float dirY = Mathf.Sin(angle * Mathf.Deg2Rad);
                Vector3 dir = new Vector3(dirX, dirY, 0f);

                EnemyBullet bullet = Managers.Pool.GetPool(bossEnemy.EnemyBulletPrefab);
                bullet.transform.position = bossEnemy.FirePoint.position;
                bullet.transform.rotation = Quaternion.FromToRotation(Vector3.right, dir);

                bullet.Initialize(dir, EnemyBullet.BulletPattern.Straight, bossEnemy.player);

                angle += angleStep;
            }
            yield return bossEnemy.CircleAttackRepeatWait;
        }
        bossEnemy.attackTimer = 0f;
        yield break;
    }
    private IEnumerator SpiralAttackCo()
    {
        float angle = 0f;
        for (int i = 0; i < bossEnemy.SpiralAttackCount; i++)
        {
            float dirX = Mathf.Cos(angle * Mathf.Deg2Rad);
            float dirY = Mathf.Sin(angle * Mathf.Deg2Rad);
            Vector3 dir = new Vector3(dirX, dirY, 0f);

            EnemyBullet bullet = Managers.Pool.GetPool(bossEnemy.EnemyBulletPrefab);
            bullet.transform.position = bossEnemy.FirePoint.position;
            bullet.transform.rotation = Quaternion.FromToRotation(Vector3.right, dir);
            bullet.Initialize(dir, EnemyBullet.BulletPattern.Straight, null);

            angle += bossEnemy.SpiralAngle;
            yield return new WaitForSeconds(0.1f);
        }
        bossEnemy.attackTimer = 0f;
        yield break;
    }
}
