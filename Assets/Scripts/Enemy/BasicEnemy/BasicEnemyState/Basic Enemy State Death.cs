using System.Collections;
using UnityEngine;

public class BasicEnemyStateDeath : BasicEnemyStateBase
{
    private void OnEnable()
    {
        Death();
        //StartCoroutine(DeathCo());
    }
    //private IEnumerator DeathCo()
    //{

    //    while (true)
    //    {
    //        AnimatorStateInfo stateInfo = refAnimator.GetCurrentAnimatorStateInfo(0);
    //        if (stateInfo.normalizedTime >= 1f) break;
    //        yield return null;
    //    }

    //    basicEnemy.Die();
    //}

    private void Death()
    {
        AnimatorStateInfo stateInfo = refAnimator.GetCurrentAnimatorStateInfo(0);

        if(stateInfo.normalizedTime >= 1f)
        {
            basicEnemy.Die();
        }
    }
}
