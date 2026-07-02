using System.Collections;
using UnityEngine;

public class EliteEnemyStateDeath : EliteEnemyStateBase
{
    private void OnEnable()
    {
        refAnimator.SetBool("isDead", true);
        StartCoroutine(DeathCo());
    }
    private IEnumerator DeathCo()
    {
        while (refAnimator.IsInTransition(0) || !refAnimator.GetCurrentAnimatorStateInfo(0).IsName("Elite Enemy Death")) yield return null;

        Managers.EnemyAudio.EliteEnemyDie();
        while (true)
        {
            AnimatorStateInfo stateInfo = refAnimator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Elite Enemy Death") && stateInfo.normalizedTime >= 1f) break;
            yield return null;
        }

        Managers.Pool.ReturnPool(eliteEnemy);
    }
}
