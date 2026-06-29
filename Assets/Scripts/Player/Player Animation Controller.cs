using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator refAnimator;

    private void OnEnable()
    {
        if(refAnimator == null)
        {
            refAnimator = GetComponentInParent<Animator>();
        }
    }

    public void OnStateChanged(PlayerStateManager.PlayerState newState)
    {
        if (refAnimator == null) return;

        if(newState == PlayerStateManager.PlayerState.Idle || newState == PlayerStateManager.PlayerState.Run)
        {
            refAnimator.SetInteger("State", (int)newState);
        }
        else if(newState == PlayerStateManager.PlayerState.Dodge)
        {
            refAnimator.SetBool("isDodged", true);
        }
        else if(newState == PlayerStateManager.PlayerState.Death)
        {
            refAnimator.SetBool("isDead", true);
        }
    }
    public AnimatorStateInfo GetCurrentStateInfo()
    {
        return refAnimator.GetCurrentAnimatorStateInfo(0);
    }
}
