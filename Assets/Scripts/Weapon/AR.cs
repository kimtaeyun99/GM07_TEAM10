using System.Collections;
using UnityEngine;

public class AR : WeaponBase, IReloadable
{
    [SerializeField] private ARBullet arBullet;

    private Coroutine shootCo;
    private Coroutine reloadCo;

    private bool isReload = false;
    public override void Shoot()
    {
        if (isReload) return;

        if (currentAmmo <= 0)
        {
            Reload();
            return;
        }

        if (shootCo == null)
        {
            shootCo = StartCoroutine(ShootCo());
        }
    }
    public override void StopShoot()
    {
        if (shootCo != null)
        {
            StopCoroutine(shootCo);
            shootCo = null;
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
        Managers.PlayerAudio.ARShoot();
    }
    public void Reload()
    {
        if (currentAmmo >= maxAmmo) return;
        if (isReload || reloadCo != null) return;
        reloadCo = StartCoroutine(ReloadCo());
    }
    public IEnumerator ReloadCo()
    {
        isReload = true;
        Managers.PlayerAudio.ARReload();
        yield return ReloadDelayWait;
        Inventory.instance.UseItemByName("AR탄창");
        currentAmmo = maxAmmo;
        isReload = false;
        reloadCo = null;
        ReloadCompleteEffect();
    }
    public void ReloadCompleteEffect()
    {
        StartCoroutine(FlashSprite());
    }

    private IEnumerator FlashSprite()
    {
        Color original = spriteRenderer.color;
        spriteRenderer.color = new Color(original.r, original.g, original.b, 0.3f);
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = original;
    }
}
