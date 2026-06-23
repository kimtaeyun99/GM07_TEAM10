using System.Collections;
using UnityEngine;

public class Pistol : WeaponBase
{
    [SerializeField] private PistolBullet pistolBullet;

    private Coroutine coroutine;
    public override void Shoot()
    {
        if (coroutine == null)
        {
            coroutine = StartCoroutine(ShootCo());
        }
    }
    public void StopShoot()
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
            FireBullet();
            yield return ShootDelayWait;
        }
    }
    public void FireBullet()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        Vector2 dir = (mousePos - firePoint.position).normalized;

        PistolBullet bullet = Managers.Pool.GetPool(pistolBullet);
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = Quaternion.FromToRotation(Vector3.right, dir);

        bullet.Fire(dir);
    }
}
