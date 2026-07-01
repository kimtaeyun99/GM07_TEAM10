using UnityEngine;
using UnityEngine.Events;
using static EliteEnemyStateManager;

public class BossEnemyStateManager : MonoBehaviour
{
    public enum BossEnemyState
    {
        None = -1, Patrol, Chase, Attack, Death
    }
    public enum BossEnemyAttackState
    {
        None = -1, NonHoming, Homing,
    }

    [SerializeField] private BossEnemy bossEnemy;
    [SerializeField] private BossEnemyState bossEnemyState = BossEnemyState.None;
    [SerializeField] private BossEnemyAttackState bossEnemyAttackState = BossEnemyAttackState.None;
    [SerializeField] private BossEnemyStateBase[] bossEnemyStates;
    [SerializeField] private BossEnemyStateAttack[] bossEnemyAttackStates;
    [SerializeField] private UnityEvent<BossEnemyState> OnStateChanged;
    [SerializeField] private UnityEvent<BossEnemyAttackState> OnAttackStateChanged;

    private BossEnemyAnimationController bossEnemyAnimationController;

    private void Awake()
    {
        if(bossEnemy == null)
        {
            bossEnemy = GetComponent<BossEnemy>();
        }
        if(bossEnemyAnimationController == null)
        {
            bossEnemyAnimationController = GetComponent<BossEnemyAnimationController>();
        }
    }
    public void SetState(BossEnemyState newState)
    {
        if (bossEnemyState == newState) return;

        for(int i=0; i<bossEnemyStates.Length; i++)
        {
            bossEnemyStates[i].enabled = false;
        }

        bossEnemyState = newState;
        bossEnemyStates[(int)newState].enabled = true;
        OnStateChanged.Invoke(bossEnemyState);
    }
    public void SetAttackState(BossEnemyAttackState newState)
    {
        if (newState == BossEnemyAttackState.None) return;
        if (bossEnemyAttackState == newState) return;

        for(int i=0;i <bossEnemyAttackStates.Length; i++)
        {
            bossEnemyAttackStates[i].enabled = false;
        }
        bossEnemyAttackState = newState;
        bossEnemyAttackStates[(int)newState].enabled = true;
        OnAttackStateChanged.Invoke(bossEnemyAttackState);
    }
    private void OnEnable()
    {
        SetState(BossEnemyState.Patrol);
        SetAttackState(BossEnemyAttackState.None);
    }
    private void Update()
    {
        if (bossEnemy.currentHp <= 0)
        {
            SetState(BossEnemyState.Death);
        }
        else if (bossEnemy.player == null)
        {
            SetState(BossEnemyState.Patrol);
        }
        else if (bossEnemy.player != null && bossEnemy.attackTimer > bossEnemy.AttackDelay && bossEnemyState != BossEnemyState.Attack)
        {
            SetState(BossEnemyState.Attack);

            if(bossEnemy.attackPattern != 4)
            {
                SetAttackState(BossEnemyAttackState.NonHoming);
            }
            else
            {
                SetAttackState(BossEnemyAttackState.Homing);
            }
        }
        else if (bossEnemy.player != null && bossEnemyState != BossEnemyState.Attack)
        {
            SetState(BossEnemyState.Chase);
        }
    }
}
