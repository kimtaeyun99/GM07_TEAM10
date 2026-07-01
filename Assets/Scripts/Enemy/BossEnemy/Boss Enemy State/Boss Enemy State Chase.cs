using System.Collections;
using UnityEngine;

public class BossEnemyStateChase : BossEnemyStateBase
{
    private Coroutine moveCo;

    private void OnEnable()
    {
        bossEnemy.attackPattern = Random.Range(0, 5);
        moveCo = StartCoroutine(MoveCo());
    }
    private void OnDisable()
    {
        StopCoroutine(moveCo);
        moveCo = null;

    }
    private void Update()
    {
        bossEnemy.dis = Vector3.Distance(bossEnemy.player.transform.position, bossEnemy.transform.position);
        bossEnemy.dir = (bossEnemy.player.transform.position - bossEnemy.transform.position).normalized;
        bossEnemy.returnPos = bossEnemy.player.transform.position - (bossEnemy.ReturnDis * bossEnemy.dir);

        if (bossEnemy.dis < bossEnemy.distanceToPlayer)
        {
            Away();
        }
    }

    private IEnumerator MoveCo()
    {

        while (true)
        {
            if (bossEnemy.dis >= bossEnemy.distanceToPlayer)
            {
                int pattern = Random.Range(0, 2);

                switch (pattern)
                {
                    case 0: yield return StartCoroutine(MoveSlowCo()); break;
                    case 1: yield return StartCoroutine(MoveDashCo()); break;
                }

                yield return ReturnPositionCo();
                yield return new WaitForSeconds(bossEnemy.MoveWaitTime);
            }
            else
            {
                yield return null;
            }
        }
    }
    private void Away()
    {
        bossEnemy.transform.position -= bossEnemy.dir * bossEnemy.moveSpeed * Time.deltaTime;
    }
    private IEnumerator MoveSlowCo()
    {
        float timer = 0f;
        while (bossEnemy.dis > 3 && timer < 5f)
        {
            bossEnemy.transform.position += bossEnemy.dir * bossEnemy.moveSpeed * 3 * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }
    }
    private IEnumerator MoveDashCo()
    {
        float timer = 0f;
        while (bossEnemy.dis > bossEnemy.distanceToPlayer && timer < 3f)
        {
            bossEnemy.transform.position += bossEnemy.dir * bossEnemy.moveSpeed * 7 * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }
    }
    private IEnumerator ReturnPositionCo()
    {
        while (Vector3.Distance(bossEnemy.transform.position, bossEnemy.returnPos) > 0.01)
        {
            bossEnemy.transform.position = Vector3.MoveTowards(bossEnemy.transform.position, bossEnemy.returnPos, bossEnemy.moveSpeed * 3 * Time.deltaTime);
            yield return null;
        }
    }
}
