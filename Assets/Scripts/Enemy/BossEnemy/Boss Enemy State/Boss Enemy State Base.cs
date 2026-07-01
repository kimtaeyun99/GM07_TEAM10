using UnityEngine;

public class BossEnemyStateBase : MonoBehaviour
{
    protected Transform refTransform;
    protected Animator refAnimator;
    //protected Rigidbody2D refRb;
    protected BossEnemyStateManager bossEnemyStateManager;
    protected BossEnemy bossEnemy;
    protected BossEnemyAnimationController bossEnemyAnimationController;

    private void Awake()
    {
        if (refTransform == null)
        {
            refTransform = GetComponent<Transform>();
        }
        if (refAnimator == null)
        {
            refAnimator = GetComponent<Animator>();
        }
        //if (refRb == null)
        //{
        //    refRb = GetComponent<Rigidbody2D>();
        //}
        if (bossEnemyStateManager == null)
        {
            bossEnemyStateManager = GetComponent<BossEnemyStateManager>();
        }
        if (bossEnemy == null)
        {
            bossEnemy = GetComponent<BossEnemy>();
        }
        if (bossEnemyAnimationController == null)
        {
            bossEnemyAnimationController = GetComponent<BossEnemyAnimationController>();
        }
    }
}
