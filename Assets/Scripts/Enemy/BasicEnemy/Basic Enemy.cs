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

    //[Header("추격 설정")]
    ////[SerializeField] public float toDistance = 5f;
    ////[SerializeField] private float detectRange = 10f;
    ////[SerializeField] private LayerMask playerLayer;
    ////[SerializeField] private LayerMask obstacle;
    ////[SerializeField] private float detectDistance = 1.0f;

    [Header("공격 설정")]
    [SerializeField] private float attackDelay = 3.0f;
    //private WaitForSeconds attackWait;
    [SerializeField] private float straightAttackCount = 3.0f;
    [SerializeField] private float straightAttackDelay = 1.0f;

    //public WaitForSeconds AttackWait { get { return attackWait; } }
    public float AttackDelay { get { return attackDelay; } }
    public float StraightAttackCount { get { return straightAttackCount; } }
    public WaitForSeconds StraightAttackWait { get; private set; }

    private void Awake()
    {
        //attackWait = new WaitForSeconds(attackDelay);
        StraightAttackWait = new WaitForSeconds(straightAttackDelay);
    }
    //private void Start()
    //{
    //    //StartCoroutine(AttackCo());
    //}

    //    if(player == null)
    //    {
    //        Collider2D hit = Physics2D.OverlapCircle(transform.position, playerDetectRange, playerLayer);
    //        if(hit != null)
    //        {
    //            player = hit.GetComponent<PlayerBase>();
    //        }
    //    }

    //    if(player != null)
    //    {
    //        dis = Vector3.Distance(player.transform.position, transform.position);
    //        dir = (player.transform.position - transform.position).normalized;

    //        if(dis > distanceToPlayer)
    //        {
    //            Move();
    //        }
    //        else
    //        {
    //            Away();
    //        }
    //    }
    //    else
    //    {
    //        Patrol();
    //    }
    //}
    //private void Move()
    //{
    //    transform.position += dir * moveSpeed * Time.deltaTime;
    //}
    //private void Away()
    //{
    //    transform.position -= dir * moveSpeed * Time.deltaTime;
    //}
    //private void Patrol()
    //{
    //    transform.position += (Vector3)patrolDir * moveSpeed * Time.deltaTime;
    //    RaycastHit2D hit = Physics2D.Raycast(transform.position, patrolDir,  obstacleDetectDistance, obstacleLayer);
    //    if (hit.collider != null)
    //    {
    //        patrolDir *= -1;
    //    }
    //private IEnumerator AttackCo()
    //{
    //    while (true)
    //    {
    //        if (player != null)
    //        {
    //            for (int i = 0; i < straightAttackCount; i++)
    //            {
    //                EnemyBullet bullet = Managers.Pool.GetPool(enemyBullet);
    //                bullet.transform.position = firePoint.position;
    //                bullet.transform.rotation = Quaternion.FromToRotation(Vector3.right, dir);
    //                bullet.Initialize(dir, EnemyBullet.BulletPattern.Straight,player);
    //                yield return straightAttackWait;
    //            }
    //            yield return attackWait;
    //        }
    //        else yield return null;
    //    }
    //}
}
