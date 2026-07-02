using System.Collections;
using UnityEngine;

public class BossEnemyStateDeath : BossEnemyStateBase
{
    private Vector3 deathPosition;
    private void OnEnable()
    {
        deathPosition = bossEnemy.transform.position;
        refAnimator.SetBool("isDead", true);
        StartCoroutine(DeathCo());
    }
    private IEnumerator DeathCo()
    {
        while (refAnimator.IsInTransition(0) || !refAnimator.GetCurrentAnimatorStateInfo(0).IsName("Boss Enemy Death")) yield return null;

        Managers.EnemyAudio.BossEnemyDie();
        while (true)
        {
            AnimatorStateInfo stateInfo = refAnimator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Boss Enemy Death") && stateInfo.normalizedTime >= 1f) break;
            yield return null;
        }

        Managers.Pool.ReturnPool(bossEnemy);
    }
    private void Update()
    {
        bossEnemy.transform.position = deathPosition;
    }
}