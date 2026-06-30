using UnityEngine;
using UnityEngine.Events;

public class EliteEnemyStateManager : MonoBehaviour
{
    public enum EliteEnemyState
    {
        None = -1, Patrol, Chase, Attack, Death
    }

    [SerializeField] private EliteEnemy eliteEnemy;
    [SerializeField] private EliteEnemyState eliteEnemyState = EliteEnemyState.None;
    [SerializeField] private EliteEnemyStateBase[] eliteEnemyStates;
    [SerializeField] private UnityEvent<EliteEnemyState> OnStateChanged;

    private EliteEnemyAnimationController eliteEnemyAnimationController;

    private void Awake()
    {
        if (eliteEnemy == null)
        {
            eliteEnemy = GetComponent<EliteEnemy>();
        }
        if (eliteEnemyAnimationController == null)
        {
            eliteEnemyAnimationController = GetComponent<EliteEnemyAnimationController>();
        }

        OnStateChanged.AddListener(eliteEnemyAnimationController.OnStateChanged);
    }
    public void SetState(EliteEnemyState newState)
    {
        if (eliteEnemyState == newState) return;
        if (eliteEnemyState != EliteEnemyState.None)
        {
            eliteEnemyStates[(int)eliteEnemyState].enabled = false;
        }
        eliteEnemyState = newState;
        eliteEnemyStates[(int)eliteEnemyState].enabled = true;
        OnStateChanged?.Invoke(eliteEnemyState);
    }
    private void OnEnable()
    {
        SetState(EliteEnemyState.Patrol);
    }

    private void Update()
    {
        if (eliteEnemy.currentHp <= 0)
        {
            SetState(EliteEnemyState.Death);
        }
        else if (eliteEnemy.player == null)
        {
            SetState(EliteEnemyState.Patrol);
        }
        else if (eliteEnemy.player != null && eliteEnemy.attackTimer > eliteEnemy.AttackDelay)
        {
            SetState(EliteEnemyState.Attack);
        }
        else if (eliteEnemy.player != null)
        {
            SetState(EliteEnemyState.Chase);
        }
    }
}
