using UnityEngine;

public class EliteEnemyAnimationController : MonoBehaviour
{
    [SerializeField] private Animator refAnimator;

    private void Awake()
    {
        if (refAnimator == null)
        {
            refAnimator = GetComponent<Animator>();
        }
    }

    public void OnStateChanged(EliteEnemyStateManager.EliteEnemyState newState)
    {
        if (refAnimator == null) return;
        if (newState == EliteEnemyStateManager.EliteEnemyState.Patrol)
        {
            refAnimator.SetBool("isPlayerDetected", false);
            refAnimator.SetInteger("State", (int)newState);
        }
        if (newState == EliteEnemyStateManager.EliteEnemyState.Chase)
        {
            refAnimator.SetBool("isPlayerDetected", true);
            refAnimator.SetInteger("State", (int)newState);
        }
        else if (newState == EliteEnemyStateManager.EliteEnemyState.Attack)
        {
            refAnimator.SetInteger("State", (int)newState);
        }
        else if (newState == EliteEnemyStateManager.EliteEnemyState.Death)
        {
            refAnimator.SetBool("isDead", true);
        }
    }
}
