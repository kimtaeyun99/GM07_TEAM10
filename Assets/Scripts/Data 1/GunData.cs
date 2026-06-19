using UnityEngine;

[CreateAssetMenu(
    fileName = "GunData",
    menuName = "Game/Gun Data"
)]
public class GunData : ScriptableObject
{
    public string weaponName;

    [Header("공격속도")]
    public float fireInterval = 0.2f;

    [Header("탄창")]
    public int magazineSize = 10;
    public float reloadTime = 1.5f;
    public int magazineCount = 5;

    [Header("발사 방식")]
    public int bulletCount = 1;
    public float spreadAngle = 0f;

    [Header("프리팹")]
    public Bullet bulletPrefab;
    public Sprite weaponSprite;
}