using UnityEngine;

public class PoolPreloader : MonoBehaviour
{
    [Header("±ÇÃÑ ÃÑ¾Ë ÇÁ¸®ÆÕ")]
    [SerializeField] private PistolBullet pistolBullet;
    [Header("±ÇÃÑ ÃÑ¾Ë ¹Ì¸® »ý¼ºÇÒ °³¼ö")]
    [SerializeField] private int pistolBulletCount = 30;

    [Header("¼¦°Ç ÃÑ¾Ë ÇÁ¸®ÆÕ")]
    [SerializeField] private ShotgunBullet shotgunBullet;
    [Header("¼¦°Ç ÃÑ¾Ë ¹Ì¸® »ý¼ºÇÒ °³¼ö")]
    [SerializeField] private int shotgunBulletCount = 30;

    [Header("ÀÚµ¿¼ÒÃÑ ÃÑ¾Ë ÇÁ¸®ÆÕ")]
    [SerializeField] private ARBullet arBullet;
    [Header("ÀÚµ¿¼ÒÃÑ ÃÑ¾Ë ¹Ì¸® »ý¼ºÇÒ °³¼ö")]
    [SerializeField] private int arBulletCount = 30;

    [Header("Basic Enemy ÇÁ¸®ÆÕ")]
    [SerializeField] private EnemyBase basicEnemy;
    [Header("Basic Enemy ¹Ì¸® »ý¼ºÇÒ °³¼ö")]
    [SerializeField] private int basicEnemyCount = 30;

    [Header("Basic Enemy Bullet ÇÁ¸®ÆÕ")]
    [SerializeField] private EnemyBullet basicEnemyBullet;
    [Header("Basic Bullet ¹Ì¸® »ý¼ºÇÒ °³¼ö")]
    [SerializeField] private int basicEnemyBulletCount = 100;

    [Header("Elite Enemy ÇÁ¸®ÆÕ")]
    [SerializeField] private EnemyBase eliteEnemy;
    [Header("Elite Enemy ¹Ì¸® »ý¼ºÇÒ °³¼ö")]
    [SerializeField] private int eliteEnemyCount = 30;

    [Header("Elite Enemy Bullet ÇÁ¸®ÆÕ")]
    [SerializeField] private EnemyBullet eliteEnemyBullet;
    [Header("Elite Bullet ¹Ì¸® »ý¼ºÇÒ °³¼ö")]
    [SerializeField] private int eliteEnemyBulletCount = 100;

    private void Start()
    {
        Managers.Pool.PreloadPool(pistolBullet, pistolBulletCount);
        Managers.Pool.PreloadPool(shotgunBullet, shotgunBulletCount);
        Managers.Pool.PreloadPool(arBullet, arBulletCount);
        Managers.Pool.PreloadPool(basicEnemy, basicEnemyCount);
        Managers.Pool.PreloadPool(basicEnemyBullet, basicEnemyBulletCount);
        Managers.Pool.PreloadPool(eliteEnemy, eliteEnemyCount);
        Managers.Pool.PreloadPool(eliteEnemyBullet, eliteEnemyBulletCount);
    }
}
