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
        AnimatorStateInfo stateInfo = playerAnimationController.GetCurrentStateInfo();
        yield return new WaitForSeconds(stateInfo.length);
        Managers.Pool.ReturnPool(this);
    }
}
