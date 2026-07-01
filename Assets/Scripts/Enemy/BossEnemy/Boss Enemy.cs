using NUnit.Framework;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;


public class BossEnemy : EnemyBase
{
    [Header("공격 기본 설정")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private EnemyBullet enemyBulletPrefab;

    [Header("다음 이동까지 대기시간")]
    [SerializeField] private float moveWaitTime = 5.0f;

    [Header("Return시 플레이어와의 거리 차이설정")]
    [SerializeField] private float returnDis = 5.0f;

    [Header("공격 설정")]
    [SerializeField] private float attackDelay;
    [Header("직선공격 설정")]
    [SerializeField] private int straightAttackCount;
    [SerializeField] private float straightAttackDelay;
    [Header("곡선공격 설정")]
    [SerializeField] private int curveAttackRepeatCount;
    [SerializeField] private float curveAttackRepeatDelay;
    [SerializeField] private int curveAttackCount;
    [SerializeField] private float curveAttackDelay;
    [Header("원형공격 설정")]
    [SerializeField] private int circleAttackRepeatCount;
    [SerializeField] private float circleAttackRepeatDelay;
    [SerializeField] private int circleAttackCount;
    [SerializeField] private float circleAttackAngleOffset;
    [Header("나선공격 설정")]
    [SerializeField] private int spiralAttackCount;
    [SerializeField] private float spiralAngle;
    [Header("유도공격 설정")]
    [SerializeField] private int homingAttackCount;
    [SerializeField] private float homingAttackDelay;

    public Transform FirePoint { get { return firePoint; } }
    public EnemyBullet EnemyBulletPrefab { get { return enemyBulletPrefab; } }
    public float MoveWaitTime { get { return moveWaitTime; } }
    public float ReturnDis { get { return returnDis; } }
    public float AttackDelay { get { return attackDelay; } }
    public WaitForSeconds AttackWait { get; private set; }
    public int StraightAttackCount { get { return straightAttackCount; } }
    public float StraightAttackDelay { get { return straightAttackDelay; } }
    public WaitForSeconds StraightAttackWait { get; private set; }
    public int CurveAttackRepeatCount { get { return curveAttackRepeatCount; } }
    public float CurveAttackRepeatDelay { get { return curveAttackRepeatDelay; } }
    public WaitForSeconds CurveAttackRepeatWait { get; private set; }
    public int CurveAttackCount { get { return curveAttackCount; } }
    public float CurveAttackDelay { get { return curveAttackDelay; } }
    public WaitForSeconds CurveAttackWait { get; private set; }
    public int CircleAttackCount { get { return circleAttackCount; } }
    public int CircleAttackRepeatCount { get { return circleAttackRepeatCount; } }
    public float CircleAttackRepeatDelay { get { return circleAttackRepeatDelay; } }
    public WaitForSeconds CircleAttackRepeatWait { get; private set; }
    public float CircleAttackAngleOffset { get { return circleAttackAngleOffset; } }
    public int SpiralAttackCount { get { return spiralAttackCount; } }
    public float SpiralAngle { get { return spiralAngle; } }
    public int HomingAttackCount { get { return homingAttackCount; } }
    public float HomingAttackDelay { get { return homingAttackDelay; } }
    public WaitForSeconds HomingAttackWait { get; private set; }
    private void Awake()
    {
        AttackWait = new WaitForSeconds(attackDelay);
        StraightAttackWait = new WaitForSeconds(straightAttackDelay);
        CurveAttackWait = new WaitForSeconds(curveAttackDelay);
        CurveAttackRepeatWait = new WaitForSeconds(curveAttackRepeatDelay);
        CircleAttackRepeatWait = new WaitForSeconds(circleAttackRepeatDelay);
        HomingAttackWait = new WaitForSeconds(homingAttackDelay);
    }

    public int attackPattern;
}
