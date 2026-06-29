using System.Collections;
using UnityEngine;

public class Shotgun : WeaponBase, IReloadable
{
    [SerializeField] private int shotgunBulletCount;
    [SerializeField] private float shotgunBulletAngle;

    [SerializeField] private ShotgunBullet shotgunBullet;

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
    private IEnumerator ShootCo()
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
    private void FireBullet()
    {
        currentAmmo--;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        Vector2 dir = (mousePos - firePoint.position).normalized;

        float baseAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        for (int i = 0; i < shotgunBulletCount; i++)
        {
            float angleOffset = shotgunBulletAngle * ((float)i / (shotgunBulletCount - 1) - 0.5f);
            float finalAngle = baseAngle + angleOffset;

            Vector2 bulletDir = new Vector2(Mathf.Cos(finalAngle * Mathf.Deg2Rad), Mathf.Sin(finalAngle * Mathf.Deg2Rad));

            ShotgunBullet bullet = Managers.Pool.GetPool(shotgunBullet);
            bullet.transform.position = firePoint.position;
            bullet.transform.rotation = Quaternion.Euler(0, 0, finalAngle);

            bullet.Fire(bulletDir);
        }
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
        //Inventory.instance.UseItemByName("¼¦°ÇÅºÃ¢");
        currentAmmo = maxAmmo;
        isReload = false;
    }
}
