using NUnit.Framework;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;


public class BossEnemy : EnemyBase
{
    [Header("공격 기본 설정")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private EnemyBullet enemyBulletPrefab;
    [SerializeField] private EnemyRaser enemyRaserPrefab;

    [Header("다음 이동메서드까지의 쿨타임")]
    [SerializeField] private float moveWaitTime = 5.0f;

    [Header("추격 설정")]
    [SerializeField] private float toDistance = 5f;
    [SerializeField] private float detectRange = 10f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float wallDetectDistance = 1.0f;
    [SerializeField] private float returnDis = 5.0f;

    [Header("공격 설정")]
    [SerializeField] private float attackDelay;
    private WaitForSeconds AttackWait;
    [Header("직선공격 설정")]
    [SerializeField] private int straightAttackCount;
    [SerializeField] private float straightAttackDelay;
    private WaitForSeconds StraightAttackWait;
    [Header("곡선공격 설정")]
    [SerializeField] private int curveAttackRepeatCount;
    [SerializeField] private float curveAttackRepeatDelay;
    private WaitForSeconds CurveAttackRepeatWait;
    [SerializeField] private int curveAttackCount;
    [SerializeField] private float curveAttackDelay;
    private WaitForSeconds CurveAttackWait;
    [Header("원형공격 설정")]
    [SerializeField] private int circleAttackBulletCount;
    [SerializeField] private int circleAttackRepeatCount;
    [SerializeField] private float circleAttackDelay;
    [SerializeField] private float circleAttackAngleOffset;
    private WaitForSeconds CircleAttackWait;
    [Header("나선공격 설정")]
    [SerializeField] private int spiralAttackCount;
    [SerializeField] private float spiralAngle;
    [Header("유도공격 설정")]
    [SerializeField] private int homingAttackCount;
    [SerializeField] private float homingAttackDelay;
    private WaitForSeconds HomingAttackWait;

    private PlayerBase player;
    private Vector3 dir;
    private float dis;
    private Vector2 patrolDir = Vector2.right;
    private Vector3 returnPos;

    private bool isUpImpossible;
    private bool isDownImpossible;
    private bool isRightImpossible;
    private bool isLeftImpossible;

    private void Awake()
    {
        AttackWait = new WaitForSeconds(attackDelay);
        StraightAttackWait = new WaitForSeconds(straightAttackDelay);
        CurveAttackWait = new WaitForSeconds(curveAttackDelay);
        CircleAttackWait = new WaitForSeconds(circleAttackDelay);
        CurveAttackRepeatWait = new WaitForSeconds(curveAttackRepeatDelay);
        HomingAttackWait = new WaitForSeconds(homingAttackDelay);
    }
    private void Start()
    {
        StartCoroutine(MoveCo());
        StartCoroutine(AttackCo());
    }

    private void Update()
    {
        if (player == null)
        {
            Collider2D hit = Physics2D.OverlapCircle(transform.position, detectRange, playerLayer);
            if (hit != null)
            {
                player = hit.GetComponent<PlayerBase>();
            }
        }
        else
        {
            dir = (player.transform.position - transform.position).normalized;
            dis = Vector3.Distance(player.transform.position, transform.position);
            returnPos = player.transform.position - (returnDis * dir);
        }
    }
    private IEnumerator MoveCo()
    {
        while (true)
        {
            if (player != null)
            {
                int pattern = Random.Range(0, 2);

                switch (pattern)
                {
                    case 0: yield return StartCoroutine(MoveSlowCo()); break;
                    case 1: yield return StartCoroutine(MoveDashCo()); break;
                    //case 2: yield return StartCoroutine(TeleportCo()); break;
                }
                yield return StartCoroutine(ReturnPositionCo());

                yield return new WaitForSeconds(moveWaitTime);
            }
            else
            {
                Patrol();
                yield return null;
            }
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
    private IEnumerator MoveDashCo()
    {
        float timer = 0f;
        while (dis > toDistance && timer < 3f)
        {
            transform.position += dir * moveSpeed * 7 * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }
    }
    //private IEnumerator TeleportCo()
    //{
    //    isUpImpossible = Physics2D.Raycast(player.transform.position, Vector2.up, 15, wallLayer);
    //    isDownImpossible = Physics2D.Raycast(player.transform.position, Vector2.down, 15, wallLayer);
    //    isRightImpossible = Physics2D.Raycast(player.transform.position, Vector2.right, 15, wallLayer);
    //    isLeftImpossible = Physics2D.Raycast(player.transform.position, Vector2.left, 15, wallLayer);

    //    List<Vector2> candir = new List<Vector2>();

    //    if (!isUpImpossible) candir.Add(Vector2.up);
    //    if (!isDownImpossible) candir.Add(Vector2.down);
    //    if (!isRightImpossible) candir.Add(Vector2.right);
    //    if (!isLeftImpossible) candir.Add(Vector2.left);

    //    if (candir == null) yield break;

    //    int teleportRandom = Random.Range(0, candir.Count);

    //    transform.position = transform.position + (Vector3)candir[teleportRandom] * 10;
        
    //    yield return null;
    //}
    private void Patrol()
    {
        transform.position += (Vector3)patrolDir * moveSpeed * Time.deltaTime;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, patrolDir, wallDetectDistance, wallLayer);
        if (hit.collider != null)
        {
            patrolDir *= -1;
        }
    }
    private IEnumerator ReturnPositionCo()
    {
        while (Vector3.Distance(transform.position, returnPos) > 0.01)
        {
            transform.position = Vector3.MoveTowards(transform.position, returnPos, moveSpeed * 3 * Time.deltaTime);
            yield return null;
        }
    }
    private IEnumerator AttackCo()
    {
        while (true)
        {
            if (player != null)
            {
                int pattern = Random.Range(0, 5);

                switch (pattern)
                {
                    case 0: yield return StartCoroutine(StraightAttackCo()); break;
                    case 1: yield return StartCoroutine(CurveAttackCo()); break;
                    case 2: yield return StartCoroutine(CircleAttackCo()); break;
                    case 3: yield return StartCoroutine(SpiralAttackCo()); break;
                    case 4: yield return StartCoroutine(HomingAttackCo()); break;
                }
                yield return AttackWait;
            }
            else yield return null;
        }
    }
    private IEnumerator StraightAttackCo()
    {
        for (int i = 0; i < straightAttackCount; i++)
        {
            EnemyBullet bullet = Managers.Pool.GetPool(enemyBulletPrefab);
            bullet.transform.position = firePoint.position;
            bullet.transform.rotation = Quaternion.FromToRotation(Vector3.right, dir);
            bullet.Initialize(dir, EnemyBullet.BulletPattern.Straight, player);
            yield return StraightAttackWait;
        }
        yield break;
    }
    private IEnumerator CurveAttackCo()
    {
        for (int i = 0; i < curveAttackRepeatCount; i++)
        {
            for (int a = 0; a < curveAttackCount; a++)
            {
                EnemyBullet bullet = Managers.Pool.GetPool(enemyBulletPrefab);
                bullet.transform.position = firePoint.position;
                bullet.transform.rotation = Quaternion.FromToRotation(Vector3.right, dir);
                bullet.Initialize(dir, EnemyBullet.BulletPattern.Curve, player);
                yield return CurveAttackWait;
            }
            yield return CurveAttackRepeatWait;
        }
        yield break;
    }
    private IEnumerator CircleAttackCo()
    {
        float angleStep = 360f / circleAttackBulletCount;
        float angle = 0f;
        for (int a = 0; a < circleAttackRepeatCount; a++)
        {
            if (a % 2 == 0)
            {
                angle += circleAttackAngleOffset;
            }
            for (int i = 0; i < circleAttackBulletCount; i++)
            {
                float dirX = Mathf.Cos(angle * Mathf.Deg2Rad);
                float dirY = Mathf.Sin(angle * Mathf.Deg2Rad);
                Vector3 dir = new Vector3(dirX, dirY, 0f);

                EnemyBullet bullet = Managers.Pool.GetPool(enemyBulletPrefab);
                bullet.transform.position = firePoint.position;
                bullet.transform.rotation = Quaternion.FromToRotation(Vector3.right, dir);

                bullet.Initialize(dir, EnemyBullet.BulletPattern.Straight, player);

                angle += angleStep;
            }
            yield return CircleAttackWait;
        }
        yield break;
    }
    private IEnumerator SpiralAttackCo()
    {
        float angle = 0f;
        for (int i = 0; i < spiralAttackCount; i++)
        {
            float dirX = Mathf.Cos(angle * Mathf.Deg2Rad);
            float dirY = Mathf.Sin(angle * Mathf.Deg2Rad);
            Vector3 dir = new Vector3(dirX, dirY, 0f);

            EnemyBullet bullet = Managers.Pool.GetPool(enemyBulletPrefab);
            bullet.transform.position = firePoint.position;
            bullet.transform.rotation = Quaternion.FromToRotation(Vector3.right, dir);
            bullet.Initialize(dir, EnemyBullet.BulletPattern.Straight, null);

            angle += spiralAngle;
            yield return new WaitForSeconds(0.1f);
        }
    }
    private IEnumerator HomingAttackCo()
    {
        for (int i = 0; i < homingAttackCount; i++)
        {
            EnemyBullet bullet = Managers.Pool.GetPool(enemyBulletPrefab);
            bullet.transform.position = firePoint.position;
            bullet.transform.rotation = Quaternion.FromToRotation(Vector3.right, dir);
            bullet.Initialize(dir, EnemyBullet.BulletPattern.Homing, player);
            yield return HomingAttackWait;
        }
        yield break;
    }
}
