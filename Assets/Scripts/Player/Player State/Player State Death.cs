using System.Collections;
using UnityEngine;

public class PlayerStateDeath : PlayerStateBase
{
    private void OnEnable()
    {
        playerBase.isDamageable = false;
        refAnimator.SetBool("isDead", true);
        StartCoroutine(DeathCo());
    }
    private IEnumerator DeathCo()
    {
        //playerBase.currentWeapon.enabled = false;

        yield return null;

        while (true)
        {
            AnimatorStateInfo stateInfo = refAnimator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Player Death") && stateInfo.normalizedTime >= 1f) break;
            yield return null;
        }

        Managers.Pool.ReturnPool(playerBase);
    }
}
