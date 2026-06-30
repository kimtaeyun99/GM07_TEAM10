using UnityEngine;

public class BasicEnemyStateBase : MonoBehaviour
{
    protected Transform refTransform;
    protected Animator refAnimator;
    protected Rigidbody2D refRb;
    protected BasicEnemyStateManager basicEnemyStateManager;
    protected BasicEnemy basicEnemy;
    protected BasicEnemyAnimationController basicEnemyAnimationController;

    private void Awake()
    {
        if(refTransform == null)
        {
            refTransform = transform;
        }
        if(refAnimator == null)
        {
            refAnimator = GetComponent<Animator>();
        }
        if (refRb == null)
        {
            refRb = GetComponent<Rigidbody2D>();
        }
        if (basicEnemyStateManager == null)
        {
            basicEnemyStateManager = GetComponent<BasicEnemyStateManager>();
        }
        if (basicEnemy == null)
        {
            basicEnemy = GetComponent<BasicEnemy>();
        }
        if (basicEnemyAnimationController == null)
        {
            basicEnemyAnimationController = GetComponent<BasicEnemyAnimationController>();
        }
    }
}
