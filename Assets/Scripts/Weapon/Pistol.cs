using System.Collections;
using UnityEngine;

public class Pistol : WeaponBase
{
    protected override void Awake()
    {
        base.Awake();
        currentAmmo = -1;
        maxAmmo = -1;
    }
    public override void Shoot()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        Vector2 dir = (mousePos - firePoint.position).normalized;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        bullet.GetComponent<Rigidbody2D>().linearVelocity = dir * 10f;
    }
}
