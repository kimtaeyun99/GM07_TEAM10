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
    protected FieldItem goldReward;
    protected int goldDropRate;
    protected FieldItem[] bulletRewards;
    protected int itemDropRate;
    protected float itemDropRadius;

    public event Action<EnemyBase> OnDead;

    public PlayerBase player;
    public Vector3 dir;
    public float dis;
    public Vector2 patrolDir = Vector2.right;
    public Vector3 returnPos;

    public float attackTimer;

    private bool isDrop = false;

    private void Update()
    {
        attackTimer += Time.deltaTime;

        LookPlayer();
    }
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
        goldReward = enemyData.GoldReward;
        goldDropRate = enemyData.GoldDropRate;
        bulletRewards = enemyData.BulletRewards;
        itemDropRate = enemyData.ItemDropRate;
        itemDropRadius = enemyData.ItemDropRadius;

    }
    public void TakeDamage(int damage)
    {
        Managers.EnemyAudio.EnemyHit();
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
        DropRewards();
    }
    public void LookPlayer()
    {
        if (player == null) return;

        if(player.transform.position.x < transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            
        }
        else
        {
            transform.rotation = Quaternion.identity;
        }
    }
    protected void DropRewards()
    {
        if (isDrop) return;
        BulletDrop();
        GoldDrop();
        isDrop = true;
    }
    protected void BulletDrop()
    {
        for (int i = 0; i < bulletRewards.Length; i++)
        {
            int rate = UnityEngine.Random.Range(0, 100);
            if (rate < itemDropRate)
            {
                Vector2 itemDropOffset = UnityEngine.Random.insideUnitCircle * itemDropRadius;
                Vector2 itemSpawnPos = (Vector2)transform.position + itemDropOffset;

                Instantiate(bulletRewards[i], itemSpawnPos, Quaternion.identity);
                Debug.Log($"{bulletRewards[i].name} 드랍");
            }
        }
    }
    protected void GoldDrop()
    {
        int rate = UnityEngine.Random.Range(0, 100);
        if (rate < goldDropRate)
        {
            Vector2 goldDropOffset = UnityEngine.Random.insideUnitCircle * itemDropRadius;
            Vector2 goldSpawnPos = (Vector2)transform.position + goldDropOffset;

            Instantiate(goldReward, goldSpawnPos, Quaternion.identity);
            Debug.Log($"{goldReward.name} 드랍");
        }
    }
}
