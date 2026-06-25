using System.Collections;
using UnityEngine;

public class AR : WeaponBase, IReloadable
{
    [SerializeField] private ARBullet arBullet;

    private Coroutine coroutine;
    private bool isReload = false;
    public override void Shoot()
    {
        if (isReload) return;

        if (currentAmmo <= 0)
        {
            Reload();
            return;
        }

        if (coroutine == null)
        {
            coroutine = StartCoroutine(ShootCo());
        }
    }
    public override void StopShoot()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }
    public IEnumerator ShootCo()
    {
        while (true)
        {
            if (Managers.Input.isAttackPressed && currentAmmo > 0 && !isReload)
            {
                FireBullet();
                yield return ShootDelayWait;
            }
            else yield return null;
        }
    }
    public void FireBullet()
    {
        currentAmmo--;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        Vector2 dir = (mousePos - firePoint.position).normalized;

        ARBullet bullet = Managers.Pool.GetPool(arBullet);
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = Quaternion.FromToRotation(Vector3.right, dir);

        bullet.Fire(dir);
    }
    public void Reload()
    {
        if (currentAmmo >= maxAmmo) return;
        StartCoroutine(ReloadCo());
    }
    public IEnumerator ReloadCo()
    {
        isReload = true;
        yield return ReloadDelayWait;
        //인벤에서 탄창 아이템 -1
        currentAmmo = maxAmmo;
        isReload = false;
    }
}
