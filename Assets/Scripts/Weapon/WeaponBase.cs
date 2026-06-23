using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class WeaponBase : MonoBehaviour
{
    [SerializeField] public string weaponName;
    [SerializeField] public int currentAmmo;
    [SerializeField] public int maxAmmo;
    [SerializeField] public float reloadDelay;
    [SerializeField] public float shootDelay;
    [SerializeField] public GameObject bulletPrefab;
    [SerializeField] public Transform firePoint;

    public WaitForSeconds ShootDelayWait;
    public WaitForSeconds ReloadDelayWait;

    protected ObjectPool<GameObject> bulletPool;
    protected virtual void Awake()
    {
        ReloadDelayWait = new WaitForSeconds(reloadDelay);
        ShootDelayWait = new WaitForSeconds(shootDelay);

        //bulletPool = new ObjectPool<GameObject>
        //(
        //    CreateBullet,
        //    OnGetBullet,
        //    OnRealeaseBullet,
        //    OnDestroyBullet,
        //    false,
        //    defaultCapacit,
        //    maxSize
        //);
    }
    public abstract void Shoot();
}
