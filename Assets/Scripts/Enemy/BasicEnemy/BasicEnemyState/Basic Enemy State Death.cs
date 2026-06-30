using System.Collections;
using UnityEngine;

public class BasicEnemyStateDeath : BasicEnemyStateBase
{
    private void OnEnable()
    {
        StartCoroutine(DeathCo());
    }
    private IEnumerator DeathCo()
    {
        while (true)
        {
            AnimatorStateInfo stateInfo = refAnimator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.normalizedTime >= 1f) break;
            yield return null;
        }

        basicEnemy.Die();
        Managers.Pool.ReturnPool(basicEnemy);
    }
}
