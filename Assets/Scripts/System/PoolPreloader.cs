using UnityEngine;

public class PoolPreloader : MonoBehaviour
{
    [Header("권총 총알 프리팹")]
    [SerializeField] private PistolBullet pistolBullet;
    [Header("권총 총알 미리 생성할 개수")]
    [SerializeField] private int pistolBulletCount = 30;

    [Header("샷건 총알 프리팹")]
    [SerializeField] private ShotgunBullet shotgunBullet;
    [Header("샷건 총알 미리 생성할 개수")]
    [SerializeField] private int shotgunBulletCount = 30;

    [Header("자동소총 총알 프리팹")]
    [SerializeField] private ARBullet arBullet;
    [Header("자동소총 총알 미리 생성할 개수")]
    [SerializeField] private int arBulletCount = 30;

    [Header("Basic Enemy 프리팹")]
    [SerializeField] private EnemyBase basicEnemy;
    [Header("Basic Enemy 미리 생성할 개수")]
    [SerializeField] private int basicEnemyCount = 30;

    [Header("Basic Enemy Bullet 프리팹")]
    [SerializeField] private EnemyBullet basicEnemyBullet;
    [Header("Basic Bullet 미리 생성할 개수")]
    [SerializeField] private int basicEnemyBulletCount = 100;

    [Header("Elite Enemy 프리팹")]
    [SerializeField] private EnemyBase eliteEnemy;
    [Header("Elite Enemy 미리 생성할 개수")]
    [SerializeField] private int eliteEnemyCount = 30;

    [Header("Elite Enemy Bullet 프리팹")]
    [SerializeField] private EnemyBullet eliteEnemyBullet;
    [Header("Elite Bullet 미리 생성할 개수")]
    [SerializeField] private int eliteEnemyBulletCount = 100;

    [Header("Boss Enemy 프리팹")]
    [SerializeField] private EnemyBase bossEnemy;
    [Header("Boss Enemy 미리 생성할 개수")]
    [SerializeField] private int bossEnemyCount = 30;

    [Header("Boss Enemy Bullet 프리팹")]
    [SerializeField] private EnemyBullet bossEnemyBullet;
    [Header("Boss Bullet 미리 생성할 개수")]
    [SerializeField] private int bossEnemyBulletCount = 100;

    private void Start()
    {
        Managers.Pool.PreloadPool(pistolBullet, pistolBulletCount);
        Managers.Pool.PreloadPool(shotgunBullet, shotgunBulletCount);
        Managers.Pool.PreloadPool(arBullet, arBulletCount);
        Managers.Pool.PreloadPool(basicEnemy, basicEnemyCount);
        Managers.Pool.PreloadPool(basicEnemyBullet, basicEnemyBulletCount);
        Managers.Pool.PreloadPool(eliteEnemy, eliteEnemyCount);
        Managers.Pool.PreloadPool(eliteEnemyBullet, eliteEnemyBulletCount);
        Managers.Pool.PreloadPool(bossEnemy, bossEnemyCount);
        Managers.Pool.PreloadPool(bossEnemyBullet, bossEnemyBulletCount);
    }
}
