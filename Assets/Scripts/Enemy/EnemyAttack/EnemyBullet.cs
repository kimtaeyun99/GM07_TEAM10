using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private PlayerBase player;

    public int damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) return;
        else if (collision.CompareTag("Wall"))
        {
            Managers.Pool.ReturnPool(this);
        }
        else if (collision.CompareTag("Destructible"))
        {
            Managers.Pool.ReturnPool(this);
        }
        else if (collision.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(damage);
            Managers.Pool.ReturnPool(this);
        }
    }
    public enum BulletPattern
    {
        None = -1, Straight, Curve, Circle, Spiral, Homing
    }

    [SerializeField] private float lifeTime = 3.0f;
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float homingSpeed = 3.0f;

    private Vector3 dir;
    private BulletPattern bulletPattern;
    private float timer;
    private float angle;
    private WaitForSeconds LifeTimeWait;
    public void Initialize(Vector3 direction, BulletPattern Pattern, PlayerBase Player)
    {
        dir = direction.normalized;
        bulletPattern = Pattern;
        player = Player;
        timer = 0f;
        angle = 0f;
        LifeTimeWait = new WaitForSeconds(lifeTime);

        StartCoroutine(LifeTimeCo());

    }
    
    private IEnumerator LifeTimeCo()
    {
        yield return LifeTimeWait;
        ReturnToPool();
    }
    private void ReturnToPool()
    {
        Managers.Pool.ReturnPool(this);
    }
    private void Update()
    {
        timer += Time.deltaTime;

        switch(bulletPattern)
        {
            case BulletPattern.Straight: StraightMove(); break;
            case BulletPattern.Curve: CurveMove(); break;
            case BulletPattern.Circle: CircleMove(); break;
            case BulletPattern.Spiral: SpiralMove(); break;
            case BulletPattern.Homing: HomingMove(); break;
        }
    }
    private void StraightMove()
    {
        transform.position += dir * speed * Time.deltaTime;
    }

    private void CurveMove()
    {
        Vector3 side = new Vector3(-dir.y, dir.x, 0f);
        Vector3 curveOffset = side * Mathf.Sin(timer * 5) * 2f;
        transform.position += (dir * speed + curveOffset) * Time.deltaTime;
    }
    private void CircleMove()
    {
        angle += 180f * Time.deltaTime;
        float rad = angle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f) * 3f;
        transform.position += offset * Time.deltaTime;
    }
    private void SpiralMove()
    {
        angle += 180f * Time.deltaTime;
        float rad = angle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f) * (1f + timer);
        transform.position += offset * Time.deltaTime;
    }
    private void HomingMove()
    {
        if (player == null)
        {
            Managers.Pool.ReturnPool(this);
            return;
        }
        dir = (player.transform.position - transform.position).normalized;
        transform.position += dir * homingSpeed * Time.deltaTime;
    }
}
