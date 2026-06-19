using UnityEngine;

[CreateAssetMenu(
    fileName = "BulletData",
    menuName = "Game/Bullet Data"
)]
public class BulletData : ScriptableObject
{
    [Header("기본 능력치")]
    public float damage = 10f;
    public float speed = 15f;
    public float lifeTime = 3f;

    [Header("관통 횟수")]
    public int pierceCount = 0;
}