using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBase : MonoBehaviour, IDamageable
{
    protected EnemyData enemyData;
    public int maxHp { get; private set; }
    public int currentHp { get; private set; }
    public int attack { get; private set; }
    public float moveSpeed { get; private set; }
    public float playerDetectRange { get; private set; }
    public LayerMask playerLayer { get; private set; }
    public float obstacleDetectDistance { get; private set; }
    public LayerMask obstacleLayer { get; private set; }
    public float distanceToPlayer { get; private set; }
    //protected ItemBase goldReward;
    //protected ItemBase[] bulletRewards;
    //protected float itemDropRadius;

    public event Action<EnemyBase> OnDead;

    public PlayerBase player;
    public Vector3 dir;
    public float dis;
    public Vector2 patrolDir = Vector2.right;
    public Vector3 returnPos;

    public void Initialize(EnemyData data)
    {
        if (data == null) return;
        enemyData = data;
        gameObject.name = enemyData.EnemyName;
        maxHp = enemyData.MaxHp;
        currentHp = maxHp;
        attack = enemyData.Attack;
        moveSpeed = enemyData.MoveSpeed;
        playerDetectRange = enemyData.PlayerDetectRange;
        playerLayer = enemyData.PlayerLayer;
        obstacleDetectDistance = enemyData.ObstacleDetectDistance;
        obstacleLayer = enemyData.ObstacleLayer;
        distanceToPlayer = enemyData.DistanceToPlayer;
        //goldReward = enemyData.GoldReward;
        //bulletRewards = enemyData.BulletRewards;
        //itemDropRadius = enemyData.ItemDropRadius;

    }
    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        Debug.Log($"{gameObject.name} 데미지 받음 ({damage}");
        Debug.Log($"{gameObject.name} 현재 체력 : {currentHp}");
        if (currentHp <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        OnDead?.Invoke(this);
        Debug.Log($"{gameObject.name} 사망");
        //Managers.Pool.ReturnPool(this);
        //DropRewards();
    }
    //protected void DropRewards()
    //{
    //    BulletDrop();
    //    GoldDrop();
    //}
    //protected void BulletDrop()
    //{
    //    for (int i = 0; i < bulletRewards.Length; i++)
    //    {
    //        //플레이어 무기 리스트를 받아서 해당되는 탄약 드랍하도록 구현
    //        //플레이어 에서 무기 관리를 어떻게 할지 받은 후 구현 
    //        int rate = Random.Range(0, 100);
    //        if (rate > 30)
    //        {
    //            Vector2 itemDropOffset = Random.insideUnitCircle * itemDropRadius;
    //            Vector2 itemSpawnPos = (Vector2)transform.position + itemDropOffset;

    //            Instantiate(bulletRewards[i], itemSpawnPos, Quaternion.identity);
    //            Debug.Log($"{bulletRewards[i].name} 드랍");
    //        }
    //    }
    //}
    //protected void GoldDrop()
    //{
    //    Vector2 goldDropOffset = Random.insideUnitCircle * itemDropRadius;
    //    Vector2 goldSpawnPos = (Vector2)transform.position + goldDropOffset;

    //    Instantiate(goldReward, goldSpawnPos, Quaternion.identity);
    //    Debug.Log($"{goldReward.name} 드랍");
    //}
}
