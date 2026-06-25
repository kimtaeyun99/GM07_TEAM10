using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class BasicEnemy : EnemyBase
{
    [Header("공격 기본 설정")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private EnemyBullet enemyBullet;

    [Header("추격 설정")]
    [SerializeField] private float toDistance = 5f;
    [SerializeField] private float detectRange = 10f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float wallDetectDistance = 1.0f;

    [Header("공격 설정")]
    [SerializeField] private float attackDelay = 3.0f;
    private WaitForSeconds AttackWait;
    [SerializeField] private float straightAttackCount = 3.0f;
    [SerializeField] private float StraightAttackDelay = 1.0f;

    private PlayerBase player;
    private Vector3 dir;
    private Vector2 patrolDir = Vector2.right;
    private float dis;
    private void Awake()
    {
        AttackWait = new WaitForSeconds(attackDelay);
    }
    private void Start()
    {
        StartCoroutine(AttackCo());
    }
    private void Update()
    {
        if(player == null)
        {
            Collider2D hit = Physics2D.OverlapCircle(transform.position, detectRange, playerLayer);
            if(hit != null)
            {
                player = hit.GetComponent<PlayerBase>();
            }
        }

        if(player != null)
        {
            dis = Vector3.Distance(player.transform.position, transform.position);
            dir = (player.transform.position - transform.position).normalized;

            if(dis > toDistance)
            {
                Move();
            }
            else
            {
                Away();
            }
        }
        else
        {
            Patrol();
        }
    }
    private void Move()
    {
        transform.position += dir * moveSpeed * Time.deltaTime;
    }
    private void Away()
    {
        transform.position -= dir * moveSpeed * Time.deltaTime;
    }
    private void Patrol()
    {
        transform.position += (Vector3)patrolDir * moveSpeed * Time.deltaTime;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, patrolDir, wallDetectDistance,wallLayer);
        if(hit.collider != null)
        {
            patrolDir *= -1;
        }
    }
    private IEnumerator AttackCo()
    {
        while (true)
        {
            if (player != null)
            {
                for (int i = 0; i < straightAttackCount; i++)
                {
                    EnemyBullet bullet = Managers.Pool.GetPool(enemyBullet);
                    bullet.transform.position = firePoint.position;
                    bullet.transform.rotation = Quaternion.FromToRotation(Vector3.right, dir);
                    bullet.Initialize(dir, EnemyBullet.BulletPattern.Straight,player);
                    yield return new WaitForSeconds(StraightAttackDelay);
                }
                yield return AttackWait;
            }
            else yield return null;
        }
    }
}
