using System.Collections;
using UnityEngine;

public class EliteEnemy : EnemyBase
{
    [Header("공격 기본 설정")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private EnemyBullet enemyBulletPrefab;
    [SerializeField] private EnemyRaser enemyRaserPrefab;

    [Header("다음 이동메서드까지의 쿨타임")]
    [SerializeField] private float moveWaitTime = 5.0f;

    //이동관련
    private Vector3 dir;
    private float dis;
    private Vector3 returnPos;
    private float angle;

    [Header("공격 설정")]
    [SerializeField] private float attackDelay;
    private WaitForSeconds AttackWait;
    [Header("직선공격 설정")]
    [SerializeField] private float straightAttackCount;
    [SerializeField] private float straightAttackDelay;
    private WaitForSeconds StraightAttackWait;
    [Header("곡선공격 설정")]
    [SerializeField] private float curveAttackCount;
    [SerializeField] private float curveAttackDelay;
    private WaitForSeconds CurveAttackWait;
    [Header("원형공격 설정")]
    [SerializeField] private float circleAttackCount;
    [SerializeField] private float circleAttackDelay;
    private WaitForSeconds CircleAttackWait;
    [Header("나선공격 설정")]
    [SerializeField] private float spiralAttackCount;
    [SerializeField] private float spiralAttackDelay;
    private WaitForSeconds SpiralAttackWait;
    [SerializeField] private float spiralAngle;
    [Header("유도공격 설정")]
    [SerializeField] private float homingAttackCount;
    [SerializeField] private float homingAttackDelay;
    private WaitForSeconds HomingAttackWait;

    //패턴 관련
    private EnemyBulletManager bulletManager;
    private void Awake()
    {
        bulletManager = GetComponent<EnemyBulletManager>();
        AttackWait = new WaitForSeconds(attackDelay);
        StraightAttackWait = new WaitForSeconds(straightAttackDelay);
        CurveAttackWait = new WaitForSeconds(curveAttackDelay);
        CircleAttackWait = new WaitForSeconds(circleAttackDelay);
        SpiralAttackWait = new WaitForSeconds(spiralAttackDelay);
        HomingAttackWait = new WaitForSeconds(homingAttackDelay);
    }
    private void Start()
    {
        StartCoroutine(MoveCo());
        StartCoroutine(AttackCo());
    }

    private void Update()
    {
        dir = (PlayerStats.Instacne.transform.position - transform.position).normalized;
        dis = Vector3.Distance(PlayerStats.Instacne.transform.position, transform.position);
        returnPos = PlayerStats.Instacne.transform.position - (10 * dir);
    }
    private IEnumerator MoveCo()
    {
        while (true)
        {
            int pattern = Random.Range(0, 3);

            switch (pattern)
            {
                case 0: yield return StartCoroutine(MoveSlowCo()); break;
                case 1: yield return StartCoroutine(MoveCurveCo()); break;
                case 2: yield return StartCoroutine(MoveDashCo()); break;
            }
            yield return StartCoroutine(ReturnPositionCo());

            yield return new WaitForSeconds(moveWaitTime);
        }
    }
    private IEnumerator MoveSlowCo()
    {
        float timer = 0f;
        while (dis > 3 && timer < 5f) 
        {
            transform.position += dir * moveSpeed * 3 * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }
    }
    private IEnumerator MoveCurveCo()
    {
        float timer = 0f;
        while (dis > 3 && timer < 5f)
        {
            angle += moveSpeed * Time.deltaTime;
            Vector3 basemove = dir * moveSpeed * Time.deltaTime;
            Vector3 side = new Vector3(-dir.y, dir.x, 0f);
            Vector3 sideOffset = side * Mathf.Sin(angle * 3f) * 6f;
            transform.position += basemove + sideOffset * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }
    }
    private IEnumerator MoveDashCo()
    {
        float timer = 0f;
        while (dis > 3 && timer < 3f)
        {
            transform.position += dir * moveSpeed * 7 * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }
    }
    private IEnumerator ReturnPositionCo()
    {
        while (Vector3.Distance(transform.position,returnPos) > 0.01)
        {
            transform.position = Vector3.MoveTowards(transform.position, returnPos, moveSpeed * 3 * Time.deltaTime);
            yield return null;
        }
    }
    private IEnumerator AttackCo()
    {
        while(true)
        {
            int pattern = Random.Range(0, 5);

            switch(pattern)
            {
                case 0: yield return StartCoroutine(StraightAttackCo()); break;
                case 1: yield return StartCoroutine(CurveAttackCo()); break;
                case 2: yield return StartCoroutine(CircleAttackCo()); break;
                case 3: yield return StartCoroutine(SpiralAttackCo()); break;
                case 4: yield return StartCoroutine(HomingAttackCo()); break;
            }
            yield return AttackWait;
        }
    }
    private IEnumerator StraightAttackCo()
    {
        for (int i = 0; i < straightAttackCount; i++)
        {
            EnemyBullet bullet = Instantiate(enemyBulletPrefab, firePoint.position, Quaternion.identity);
            bullet.Initialize(dir, EnemyBullet.BulletPattern.Straight);
            yield return StraightAttackWait;
        }
        yield return null;
    }
    private IEnumerator CurveAttackCo()
    {
        for (int i=0; i< curveAttackCount; i++)
        {
            EnemyBullet bullet = Instantiate(enemyBulletPrefab, firePoint.position, Quaternion.identity);
            bullet.Initialize(dir, EnemyBullet.BulletPattern.Curve);
            yield return CurveAttackWait;
        }
        yield return null;
    }
    private IEnumerator CircleAttackCo()
    {
         bulletManager.FireCirclePattern((int)circleAttackCount);
        yield return null;
    }
    private IEnumerator SpiralAttackCo()
    {
        float offset = Time.time * spiralAngle;
        bulletManager.FireSpiralPattern((int)spiralAttackCount, offset);
        yield return null;
    }
    private IEnumerator HomingAttackCo()
    {
        for (int i = 0; i < homingAttackCount; i++)
        {
            EnemyBullet bullet = Instantiate(enemyBulletPrefab, firePoint.position, Quaternion.identity);
            bullet.Initialize(dir, EnemyBullet.BulletPattern.Homing);
            yield return HomingAttackWait;
        }
        yield return null;
    }
}