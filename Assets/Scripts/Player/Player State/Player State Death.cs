using System.Collections;
using UnityEngine;

public class PlayerStateDeath : PlayerStateBase
{
    private void OnEnable()
    {
        playerBase.isDamageable = false;
        StartCoroutine(DeathCo());
    }
    private IEnumerator DeathCo()
    {
        refAnimator.SetBool("isDead", true);

        while (true)
        {
            AnimatorStateInfo stateInfo = refAnimator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.normalizedTime >= 1f) break;
            yield return null;
        }

        Managers.Pool.ReturnPool(playerBase);
    }
}
