using UnityEngine;

public class BasicEnemyAnimationController : MonoBehaviour
{
    [SerializeField] private Animator refAnimator;

    private void Awake()
    {
        if(refAnimator == null)
        {
            refAnimator = GetComponent<Animator>();
        }
    }

    public void OnStateChanged(BasicEnemyStateManager.BasicEnemyState newState)
    {
        if (refAnimator == null) return;
        if (newState == BasicEnemyStateManager.BasicEnemyState.Patrol)
        {
            refAnimator.SetBool("isPlayerDetected", false);
            refAnimator.SetInteger("State", (int) newState);
        }
        if (newState == BasicEnemyStateManager.BasicEnemyState.Chase)
        {
            refAnimator.SetBool("isPlayerDetected", true);
            refAnimator.SetInteger("State", (int) newState);
        }
        else if (newState == BasicEnemyStateManager.BasicEnemyState.Attack)
        {
            refAnimator.SetInteger("State", (int) newState);
        }
        else if (newState == BasicEnemyStateManager.BasicEnemyState.Death)
        {
            refAnimator.SetTrigger("isDead");
        }
    }
}
