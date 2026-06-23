using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    public string weaponName;
    public int currentAmmo;
    public int maxAmmo;
    public float reloadDelay;
    public float shootDelay;

    public abstract void Shoot();
}
