using UnityEngine;

public class BasicEnemyStateBase : MonoBehaviour
{
    protected Transform refTransform;
    protected Animator refAnimator;
    protected BasicEnemyStateManager basicEnemyStateManager;
    protected BasicEnemy basicEnemy;
    protected BasicEnemyAnimationController basicEnemyAnimationController;

    private void Awake()
    {
        if(refTransform == null)
        {
            refTransform = GetComponent<Transform>();
        }
        if(refAnimator == null)
        {
            refAnimator = GetComponent<Animator>();
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
