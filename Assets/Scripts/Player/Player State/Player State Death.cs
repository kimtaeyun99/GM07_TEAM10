using System.Collections;
using UnityEngine;

public class PlayerStateDeath : PlayerStateBase
{
    private void OnEnable()
    {
        if (playerBase.currentWeapon != null)
        {
            playerBase.currentWeapon.gameObject.SetActive(false);
        }
        playerBase.isDamageable = false;
        refAnimator.SetBool("isDead", true);
        StartCoroutine(DeathCo());
    }
    private IEnumerator DeathCo()
    {
        Managers.PlayerAudio.PlayerDead();

        yield return null;

        while (true)
        {
            AnimatorStateInfo stateInfo = refAnimator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Player Death") && stateInfo.normalizedTime >= 1f) break;
            yield return null;
        }

        if(IsTutorialScene())
        {
            Destroy(playerBase.gameObject);
        }
        else
        {
            Managers.Pool.ReturnPool(playerBase);
        }
    }
    private bool IsTutorialScene()
    {
        return UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Stage_Tutorial";
    }
}
