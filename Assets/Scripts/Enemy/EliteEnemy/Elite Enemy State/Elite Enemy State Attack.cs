using UnityEngine;

public class EliteEnemyStateAttack : EliteEnemyStateBase
{
    private void Update()
    {
        eliteEnemy.dis = Vector3.Distance(eliteEnemy.player.transform.position, eliteEnemy.transform.position);
        eliteEnemy.dir = (eliteEnemy.player.transform.position - eliteEnemy.transform.position).normalized;
    }
}
