using UnityEngine;

public class PoolPreloader : MonoBehaviour
{
    [Header("프리팹")]
    [SerializeField] private PistolBullet pistolBullet;

    [Header("미리 생성할 개수")]
    [SerializeField] private int pistolBulletCount = 30;

    private void Start()
    {
        Managers.Pool.PreloadPool(pistolBullet, pistolBulletCount);
    }
}
