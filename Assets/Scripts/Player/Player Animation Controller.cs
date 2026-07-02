using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator refAnimator;

    private void Awake()
    {
        if(refAnimator == null)
        {
            refAnimator = GetComponent<Animator>();
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
            refAnimator.SetTrigger("isDodged");
        }
        else if(newState == PlayerStateManager.PlayerState.Death)
        {
            refAnimator.SetBool("isDead", true);
        }
    }
}
