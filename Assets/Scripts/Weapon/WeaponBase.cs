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
    [SerializeField] public Transform firePoint;
    [SerializeField] public SpriteRenderer spriteRenderer;
    [SerializeField] public float flashDuration = 0.5f;

    public WaitForSeconds ShootDelayWait;
    public WaitForSeconds ReloadDelayWait;
    private void OnEnable()
    {
        ReloadDelayWait = new WaitForSeconds(reloadDelay);
        ShootDelayWait = new WaitForSeconds(shootDelay);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public abstract void Shoot();
    public abstract void StopShoot();
}
