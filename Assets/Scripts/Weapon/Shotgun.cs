using System.Collections;
using UnityEngine;

public class Shotgun : WeaponBase, IReloadable
{
    [SerializeField] private float bulletCount;
    protected override void Awake()
    {
        base.Awake();
        currentAmmo = 10;
        maxAmmo = 10;
    }
    public override void Shoot()
    {
        currentAmmo -= 1;
    }
    public void Reload()
    {
        if (currentAmmo >= maxAmmo) return;
        StartCoroutine(ReloadCo());
    }
    public IEnumerator ReloadCo()
    {
        yield return ReloadDelayWait;
        //인벤에서 탄창 아이템 -1
        currentAmmo = maxAmmo;
    }
}
