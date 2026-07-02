using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "GameData/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("Enemy 정보")]
    [SerializeField] private string enemyName;

    [Header("Enemy 능력치")]
    [SerializeField] private int maxHp = 30;
    [SerializeField] private int attack = 5;
    [SerializeField] private float moveSpeed = 1.0f;

    [Header("탐지관련")]
    [SerializeField] private float playerDetectRange = 10f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float obstacleDetectDistance = 1.0f;
    [SerializeField] private LayerMask obstacleLayer;

    [Header("이동관련")]
    [SerializeField] private float distanceToPlayer = 5f;

    [Header("Enemy 처치 보상")]
    [SerializeField] private FieldItem goldReward;
    [SerializeField] private int goldDropRate;
    [SerializeField] private FieldItem[] bulletRewards;
    [SerializeField] private int itemDropRate;
    [SerializeField] private float itemDropRadius;
 

    public string EnemyName { get { return enemyName; } }
    public int MaxHp { get { return maxHp; } }
    public int Attack { get { return attack; } }
    public float MoveSpeed { get { return moveSpeed; } }

    public float PlayerDetectRange { get { return playerDetectRange; } }

    public LayerMask PlayerLayer { get { return playerLayer; } }
    public float ObstacleDetectDistance { get { return obstacleDetectDistance; } }
    public LayerMask ObstacleLayer { get { return obstacleLayer; } }
    public float DistanceToPlayer { get { return distanceToPlayer; } }
    public FieldItem GoldReward { get { return goldReward; } }
    public int GoldDropRate { get { return goldDropRate; } }
    public FieldItem[] BulletRewards { get { return bulletRewards; } }
    public int ItemDropRate { get { return itemDropRate; } }
    public float ItemDropRadius { get { return itemDropRadius; } }
}
