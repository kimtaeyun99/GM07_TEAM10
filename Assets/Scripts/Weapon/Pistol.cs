using System.Collections;
using UnityEngine;

public class Pistol : WeaponBase
{
    [SerializeField] public string WeaponName;
    [SerializeField] public float ReloadDelay;
    [SerializeField] public float ShootDelay;

    public WaitForSeconds ShootDelayWait;
    public WaitForSeconds ReloadDelayWait;

    [SerializeField] public GameObject bulletPrefab;
    [SerializeField] public Transform firePoint;
    private void Awake()
    {
        weaponName = WeaponName;
        currentAmmo = -1;
        maxAmmo = -1;
        reloadDelay = ReloadDelay;
        shootDelay = ShootDelay;
        ReloadDelayWait = new WaitForSeconds(reloadDelay);
        ShootDelayWait = new WaitForSeconds(shootDelay);
    }
    public override void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }

}
