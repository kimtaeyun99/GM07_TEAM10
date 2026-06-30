using UnityEngine;

public class EliteEnemyStateBase : MonoBehaviour
{
    protected Transform refTransform;
    protected Animator refAnimator;
    protected Rigidbody2D refRb;
    protected EliteEnemyStateManager eliteEnemyStateManager;
    protected EliteEnemy eliteEnemy;
    protected EliteEnemyAnimationController eliteEnemyAnimationController;

    private void Awake()
    {
        if (refTransform == null)
        {
            refTransform = transform;
        }
        if (refAnimator == null)
        {
            refAnimator = GetComponent<Animator>();
        }
        if (refRb == null)
        {
            refRb = GetComponent<Rigidbody2D>();
        }
        if (eliteEnemyStateManager == null)
        {
            eliteEnemyStateManager = GetComponent<EliteEnemyStateManager>();
        }
        if (eliteEnemy == null)
        {
            eliteEnemy = GetComponent<EliteEnemy>();
        }
        if (eliteEnemyAnimationController == null)
        {
            eliteEnemyAnimationController = GetComponent<EliteEnemyAnimationController>();
        }
    }
}
