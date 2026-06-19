using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D rb;
    protected BulletData data;  // 총알의 스크립터블 오브젝트
    protected float damage;    //  데미지
    protected int remainingPierce; // 총알 관통
    protected GameObject owner;  // 총알을 발사한 게임오브젝트 (적, 플레이어 등)

    protected BulletPool bulletPool; // 현재 총알을 관리하는 오브젝트 풀
    private Coroutine lifeTimeCoroutine; // 총알의 수명 코루틴
    private bool isReturned; // 총알이 풀에 중복으로 반환되는것을 방지

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetPool(BulletPool pool)
    {
        bulletPool = pool;
    }

    public virtual void Initialize(BulletData bulletData, Vector2 direction, GameObject bulletOwner, float damageMultiplier = 1f) //(나중에 아이템같은걸 먹었을때, 총알의 데미지를 공식을 여기에 넣어주면됨)
    {
        owner = bulletOwner;
        data = bulletData;

        damage = data.damage * damageMultiplier; // 대미지 배율 설정 
        remainingPierce = data.pierceCount; // 관통 할수있는수 (나중에 총알에 효과같은걸 붙일예정 ex)적관통가능,벽튕기기 등)

        isReturned = false; // 풀에서 다시 꺼냈으므로 반환 상태 초기화

        rb.linearVelocity = Vector2.zero; // 이전에 총알이 가지고있던 속도 초기화
        rb.angularVelocity = 0f; // 이전에 총알이 가지고있던 회전속도 초기화

        rb.linearVelocity = direction.normalized * data.speed;  // 해당 방향으로 ScriptableObject BulletData의 속도만큼 이동, 대각이동빨라짐 방지하기위해 방향에 normalized

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle);   //해당 총알 오브젝트의 회전값을 방향에 맞게 설정 현재는  총알이 구 형태여서 상관없음

        if (lifeTimeCoroutine != null) // 이전에 실행중이던 수명 코루틴이 있으면 정지
        {
            StopCoroutine(lifeTimeCoroutine);
        }

        lifeTimeCoroutine = StartCoroutine(LifeTimeCoroutine()); // ScriptableObject BulletData의 lifeTime시간이 지나면 풀에 반환
    }

    private IEnumerator LifeTimeCoroutine()
    {
        yield return new WaitForSeconds(data.lifeTime);

        ReturnToPool();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == owner) return; // 총알을 발사한 GameObject와 접촉시 무시

        if (collision.tag == "Player") return; // 플레이어면 리턴

        if (collision.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(damage);

            OnHit(collision);   // 나중에 특수총알 ex) 폭팔, 연쇄 등 특수효과를 넣을때 따로 실행되는 로직

            if (remainingPierce > 0)    // 현재총알의 남은 관통수치 나중에 기능추가할려고 넣어둠 현재는 아무영향 안끼침
            {
                remainingPierce--;
            }
            else
            {
                ReturnToPool();
            }
        }
    }

    protected virtual void OnHit(Collider2D collision)
    {

    }

    protected virtual void ReturnToPool()
    {
        if (isReturned) return; // 이미 풀에 반환된 총알이면 중복 반환 방지

        isReturned = true;

        if (lifeTimeCoroutine != null) // 실행중인 총알 수명 코루틴 정지
        {
            StopCoroutine(lifeTimeCoroutine);
            lifeTimeCoroutine = null;
        }

        rb.linearVelocity = Vector2.zero; // 총알의 이동속도 초기화
        rb.angularVelocity = 0f; // 총알의 회전속도 초기화

        owner = null; // 총알을 발사한 오브젝트 정보 초기화
        data = null; // 총알 데이터 초기화
        damage = 0f; // 총알 데미지 초기화
        remainingPierce = 0; // 총알 관통 횟수 초기화

        if (bulletPool != null)
        {
            bulletPool.ReturnBullet(this); // 총알을 오브젝트 풀에 반환
        }
        else
        {
            gameObject.SetActive(false); // 연결된 풀이 없으면 비활성화
        }
    }
}