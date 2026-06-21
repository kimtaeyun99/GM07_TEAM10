using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public int damage = 10;

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Enemy")) return;

    //    if (collision.TryGetComponent(out IDamageable damageable))
    //    {
    //        if (PlayerStats.Instance.IsDodge) return;
    //        damageable.TakeDamage(damage);
    //    }
    //}
    public enum BulletPattern
    {
        None = -1, Straight, Curve, Circle, Spiral, Homing
    }
    [SerializeField] private float lifeTime = 3.0f;
    [SerializeField] private float speed = 5.0f;

    private Vector3 dir;
    private BulletPattern bulletPattern;
    private float timer;
    private float angle;
    public void Initialize(Vector3 direction, BulletPattern Pattern)
    {
        dir = direction.normalized;
        bulletPattern = Pattern;
    }
    private void Start()
    {
        Destroy(gameObject, lifeTime);
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
            //case BulletPattern.Homing: HomingMove(); break;
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
    //private void HomingMove()
    //{
    //    dir = (PlayerStats.Instacne.transform.position - transform.position).normalized;
    //    transform.position += dir * speed * Time.deltaTime;
    //}
}
