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
        //refAnimator.SetBool("isDead", false);
        //AnimatorStateInfo stateInfo = playerAnimationController.GetCurrentStateInfo();
        //float duration = stateInfo.length / stateInfo.speed;
        //yield return new WaitForSeconds(duration);
        //Managers.Pool.ReturnPool(playerBase);

        refAnimator.SetBool("isDead", true);

        // Death 애니메이션 끝날 때까지 대기
        while (true)
        {
            AnimatorStateInfo stateInfo = refAnimator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Death") && stateInfo.normalizedTime >= 1f)
                break;
            yield return null;
        }

        Managers.Pool.ReturnPool(playerBase);
    }
}
