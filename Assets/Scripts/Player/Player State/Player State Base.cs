using UnityEngine;

public class PlayerStateBase : MonoBehaviour
{
    protected Transform refTransform;
    protected CharacterController refCharacterController;
    protected Animator refAnimator;
    protected PlayerStateManager playerStateManager;
    protected PlayerAnimationController playerAnimationController;
    protected Rigidbody2D refRb;
    protected PlayerBase playerBase;

    private void OnEnable()
    {
        if(refTransform == null)
        {
            refTransform = transform;
        }
        if(refCharacterController == null)
        {
            refCharacterController = GetComponent<CharacterController>();
        }
        if(refAnimator == null)
        {
            refAnimator = GetComponent<Animator>();
        }
        if(playerStateManager == null)
        {
            playerStateManager = GetComponent<PlayerStateManager>();
        }
        if(playerAnimationController == null)
        {
            playerAnimationController = GetComponentInChildren<PlayerAnimationController>();
        }
        if(refRb == null)
        {
            refRb = GetComponent<Rigidbody2D>();
        }
        if(playerBase == null)
        {
            playerBase = GetComponent<PlayerBase>();
        }
    }
}
