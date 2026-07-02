using System.Collections;
using UnityEngine;

public class EliteEnemyStateChase : EliteEnemyStateBase
{
    private Coroutine moveCo;

    private void OnEnable()
    {
        moveCo = StartCoroutine(MoveCo());
    }
    private void OnDisable()
    {
        StopCoroutine(moveCo);
        moveCo = null;

    }
    private void Update()
    {
        eliteEnemy.dis = Vector3.Distance(eliteEnemy.player.transform.position, eliteEnemy.transform.position);
        eliteEnemy.dir = (eliteEnemy.player.transform.position - eliteEnemy.transform.position).normalized;
        eliteEnemy.returnPos = eliteEnemy.player.transform.position - (eliteEnemy.ReturnDis * eliteEnemy.dir);

        if(eliteEnemy.dis < eliteEnemy.distanceToPlayer)
        {
            Away();
        }
    }

    private IEnumerator MoveCo()
    {

        while(true)
        {
            if(eliteEnemy.dis >= eliteEnemy.distanceToPlayer)
            {
                int pattern = Random.Range(0, 2);

                switch (pattern)
                {
                    case 0: yield return StartCoroutine(MoveSlowCo()); break;
                    case 1: yield return StartCoroutine(MoveDashCo()); break;
                }

                yield return ReturnPositionCo();
                yield return new WaitForSeconds(eliteEnemy.MoveWaitTime);
            }
            else
            {
                yield return null;
            }
        }
    }
    private void Away()
    {
        eliteEnemy.transform.position -= eliteEnemy.dir * eliteEnemy.moveSpeed * Time.deltaTime;
    }
    private IEnumerator MoveSlowCo()
    {
        float timer = 0f;
        while (eliteEnemy.dis > 3 && timer < 5f)
        {
            eliteEnemy.transform.position += eliteEnemy.dir * eliteEnemy.moveSpeed * 3 * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }
    }
    private IEnumerator MoveDashCo()
    {
        float timer = 0f;
        while (eliteEnemy.dis > eliteEnemy.distanceToPlayer && timer < 3f)
        {
            eliteEnemy.transform.position += eliteEnemy.dir * eliteEnemy.moveSpeed * 7 * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }
    }
    private IEnumerator ReturnPositionCo()
    {
        while (Vector3.Distance(eliteEnemy.transform.position, eliteEnemy.returnPos) > 0.01)
        {
            eliteEnemy.transform.position = Vector3.MoveTowards(eliteEnemy.transform.position, eliteEnemy.returnPos, eliteEnemy.moveSpeed * 3 * Time.deltaTime);
            yield return null;
        }
    }
}
