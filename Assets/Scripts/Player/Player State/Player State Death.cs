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
        refAnimator.SetBool("isDead", false);
        AnimatorStateInfo stateInfo = playerAnimationController.GetCurrentStateInfo();
        float duration = stateInfo.length / stateInfo.speed;
        yield return new WaitForSeconds(duration);
        //refAnimator.SetBool("isDead", false);
        //Managers.Pool.ReturnPool(playerBase);
    }
}
