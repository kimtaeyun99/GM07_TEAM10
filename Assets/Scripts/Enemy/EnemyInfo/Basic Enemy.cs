using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class BasicEnemy : EnemyBase
{
    [Header("공격 기본 설정")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private EnemyBullet enemyBulletPrefab;

    [Header("이동 설정")]
    //[SerializeField] private float toDistance = 5;

    [Header("공격 설정")]
    [SerializeField] private float attackDelay = 3.0f;
    private WaitForSeconds AttackWait;
    [SerializeField] private float straightAttackCount;
    [SerializeField] private float StraightAttackDelay = 1.0f;

    private float dis;
    private Vector3 dir;
    private void Awake()
    {
        AttackWait = new WaitForSeconds(attackDelay);
    }
    private void Start()
    {
        StartCoroutine(AttackCo());
    }
    //private void Update()
    //{
    //    //dis = Vector3.Distance(PlayerStats.Instacne.transform.position, transform.position);

    //    dir = (PlayerStats.Instacne.transform.position - transform.position).normalized;
    //    if (dis > toDistance)
    //    {
    //        Move();
    //    }
    //}
    private void Move()
    {
        transform.position += dir * moveSpeed * Time.deltaTime;
    }
    private IEnumerator AttackCo()
    {
        while (true)
        {
            for (int i = 0; i < straightAttackCount; i++)
            {
                EnemyBullet bullet = Instantiate(enemyBulletPrefab, firePoint.position, Quaternion.identity);
                bullet.Initialize(dir, EnemyBullet.BulletPattern.Straight);
                yield return new WaitForSeconds(StraightAttackDelay);
            }
            yield return AttackWait;
        }
    }
}
