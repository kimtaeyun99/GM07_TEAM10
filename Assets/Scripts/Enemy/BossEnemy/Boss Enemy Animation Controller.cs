using UnityEngine;

public class BossEnemyAnimationController : MonoBehaviour
{
    [SerializeField] private Animator refAnimator;

    private void Awake()
    {
        if(refAnimator == null)
        {
            refAnimator = GetComponent<Animator>();
        }
    }

    public void OnStateChanged(BossEnemyStateManager.BossEnemyState newState)
    {
        if (refAnimator == null) return;

        if(newState == BossEnemyStateManager.BossEnemyState.Patrol)
        {
            refAnimator.SetBool("isPlayerDetected", false);
            refAnimator.SetInteger("State", (int)newState);
        }
        else if(newState == BossEnemyStateManager.BossEnemyState.Chase)
        {
            refAnimator.SetBool("isPlayerDetected", true);
            refAnimator.SetInteger("State", (int)newState);
        }
        else if(newState == BossEnemyStateManager.BossEnemyState.Attack)
        {
            refAnimator.SetInteger("State", (int)newState);
        }
        else if(newState == BossEnemyStateManager.BossEnemyState.Death)
        {
            refAnimator.SetBool("isDead", true);
        }
    }
    public void OnAttackStateChanged(BossEnemyStateManager.BossEnemyAttackState newState)
    {
        if (refAnimator == null) return;
        refAnimator.SetInteger("AttackState", (int)newState);
    }
}
