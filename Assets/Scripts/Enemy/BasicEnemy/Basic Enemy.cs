using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class BasicEnemy : EnemyBase
{
    [Header("공격 기본 설정")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private EnemyBullet enemyBullet;

    public Transform FirePoint { get { return firePoint; } }
    public EnemyBullet EnemyBullet { get { return enemyBullet; } }

    [Header("공격 설정")]
    [SerializeField] private float attackDelay = 3.0f;
    [SerializeField] private float straightAttackCount = 3.0f;
    [SerializeField] private float straightAttackDelay = 1.0f;

    public float AttackDelay { get { return attackDelay; } }
    public float StraightAttackCount { get { return straightAttackCount; } }
    public WaitForSeconds StraightAttackWait { get; private set; }

    private void Awake()
    {
        StraightAttackWait = new WaitForSeconds(straightAttackDelay);
    }
}
