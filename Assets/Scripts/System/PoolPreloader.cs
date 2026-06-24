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

    [Header("Enemy Bullet ÇÁ¸®ÆÕ")]
    [SerializeField] private EnemyBullet enemyBullet;
    [Header("Enemy Bullet ¹Ì¸® »ý¼ºÇÒ °³¼ö")]
    [SerializeField] private int enemyBulletCount = 100;
    private void Start()
    {
        Managers.Pool.PreloadPool(pistolBullet, pistolBulletCount);
        Managers.Pool.PreloadPool(shotgunBullet, shotgunBulletCount);
        Managers.Pool.PreloadPool(arBullet, arBulletCount);
        Managers.Pool.PreloadPool(basicEnemy, basicEnemyCount);
        Managers.Pool.PreloadPool(enemyBullet, enemyBulletCount);
    }
}
