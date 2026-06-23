using UnityEngine;

public class ARBullet : PlayerBulletBase
{
    private Vector2 direction;
    public void Fire(Vector2 dir)
    {
        direction = dir;
    }
    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }
}
