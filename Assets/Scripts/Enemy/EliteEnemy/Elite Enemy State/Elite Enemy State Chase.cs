using UnityEngine;

public class EliteEnemyStateChase : EliteEnemyStateBase
{
    private void Update()
    {
        eliteEnemy.dis = Vector3.Distance(eliteEnemy.player.transform.position, eliteEnemy.transform.position);
        eliteEnemy.dir = (eliteEnemy.player.transform.position - eliteEnemy.transform.position).normalized;
        eliteEnemy.returnPos = eliteEnemy.player.transform.position - (eliteEnemy.ReturnDis * eliteEnemy.dir);
    }
}
