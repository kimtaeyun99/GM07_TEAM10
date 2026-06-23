using System.Collections;
using UnityEngine;

public class PlayerBulletBase : MonoBehaviour
{
    [SerializeField] public int damage;
    [SerializeField] public int speed;
    [SerializeField] public float returnBulletTime = 3f;
    public WaitForSeconds returnBulletWait;

    private Coroutine coroutine;
    private void Awake()
    {
        returnBulletWait = new WaitForSeconds(returnBulletTime);
    }
    private void OnEnable()
    {
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(ReturnBulletCo());
    }
    private void OnDisable()
    {
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }
    private IEnumerator ReturnBulletCo()
    {
        yield return returnBulletWait;
        Managers.Pool.ReturnPool(this);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) return;
        else if (collision.CompareTag("Enemy"))
        {
            collision.TryGetComponent(out IDamageable damageable);
            damageable.TakeDamage(damage);
            Managers.Pool.ReturnPool(this);
        }
        else if (collision.CompareTag("Wall"))
        {
            Managers.Pool.ReturnPool(this);
        }
    }
}
