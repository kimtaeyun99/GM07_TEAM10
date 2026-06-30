using UnityEngine;
using UnityEngine.Events;

public class BasicEnemyStateManager : MonoBehaviour
{
    public enum BasicEnemyState
    {
        None = -1, Patrol, Chase, Attack, Death
    }

    [SerializeField] private BasicEnemy basicEnemy;
    [SerializeField] private BasicEnemyState basicEnemyState = BasicEnemyState.None;
    [SerializeField] private BasicEnemyStateBase[] basicEnemyStates;
    [SerializeField] private UnityEvent<BasicEnemyState> OnStateChanged;

    private BasicEnemyAnimationController basicEnemyAnimationController;

    private void Awake()
    {
        if(basicEnemy == null)
        {
            basicEnemy = GetComponent<BasicEnemy>();
        }
        if(basicEnemyAnimationController == null)
        {
            basicEnemyAnimationController = GetComponent<BasicEnemyAnimationController>();
        }

        OnStateChanged.AddListener(basicEnemyAnimationController.OnStateChanged);
    }
    public void SetState(BasicEnemyState newState)
    {
        if (basicEnemyState == newState) return;
        if (basicEnemyState != BasicEnemyState.None)
        {
            basicEnemyStates[(int)basicEnemyState].enabled = false;
        }
        basicEnemyState = newState;
        basicEnemyStates[(int)basicEnemyState].enabled = true;
        OnStateChanged?.Invoke(basicEnemyState);
    }
    private void OnEnable()
    {
        SetState(BasicEnemyState.Patrol);
    }

    private void Update()
    {
        if(basicEnemy.currentHp <= 0)
        {
            SetState(BasicEnemyState.Death);
        }
        else if(basicEnemy.player == null)
        {
            SetState(BasicEnemyState.Patrol);
        }
        else if (basicEnemy.player != null && basicEnemy.attackTimer > basicEnemy.AttackDelay)
        {
            SetState(BasicEnemyState.Attack);
        }
        else if(basicEnemy.player != null)
        {
            SetState(BasicEnemyState.Chase);
        }
    }

}
