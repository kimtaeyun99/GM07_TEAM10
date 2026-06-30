using System.Collections;
using UnityEngine;

public class BasicEnemyStateDeath : BasicEnemyStateBase
{
    private void OnEnable()
    {
        refAnimator.SetBool("isDead", true);
        StartCoroutine(DeathCo());
    }
    private IEnumerator DeathCo()
    {
        while (refAnimator.IsInTransition(0) || !refAnimator.GetCurrentAnimatorStateInfo(0).IsName("Basic Enemy Death"))
            yield return null;

        while (true)
        {
            AnimatorStateInfo stateInfo = refAnimator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Basic Enemy Death") && stateInfo.normalizedTime >= 1f) break;
            yield return null;
        }

        Managers.Pool.ReturnPool(basicEnemy);
    }
}
