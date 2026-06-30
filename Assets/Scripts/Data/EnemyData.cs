using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "GameData/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("Enemy 정보")]
    [SerializeField] private string enemyName;
    [SerializeField] private EnemyBase enemyPrefab;

    [Header("Enemy Sprite")]
    [SerializeField] private Sprite enemySprite;

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

    //[Header("Enemy 처치 보상")]
    //[SerializeField] private ItemBase goldReward;
    //[SerializeField] private ItemBase[] bulletRewards;
    //[SerializeField] private float itemDropRadius;

    public string EnemyName { get { return enemyName; } }
    public EnemyBase EnemyPrefab {  get { return enemyPrefab; } }
    public Sprite EnemySprite { get { return enemySprite; } }
    public int MaxHp { get { return maxHp; } }
    public int Attack { get { return attack; } }
    public float MoveSpeed { get { return moveSpeed; } }

    public float PlayerDetectRange { get { return playerDetectRange; } }

    public LayerMask PlayerLayer { get { return playerLayer; } }
    public float ObstacleDetectDistance { get { return obstacleDetectDistance; } }
    public LayerMask ObstacleLayer { get { return obstacleLayer; } }
    public float DistanceToPlayer { get { return distanceToPlayer; } }
    //public ItemBase GoldReward { get { return goldReward; } }
    //public ItemBase[] BulletRewards { get { return bulletRewards; } }
    //public float ItemDropRadius { get { return itemDropRadius; } }
}
