using UnityEngine;

public class DestructibleObject : MonoBehaviour , IDamageable
{
    [SerializeField] private int maxHp = 3; // 파괴 가능한 오브젝트 최대 체력
    [SerializeField] private GameObject dropPrefab; // 파괴 시 드랍할 오브젝트

    private int currentHp; // 현재 체력

    private void Awake()
    {
        currentHp = maxHp; // 시작할 때 현재 체력을 최대 체력으로 설정
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage; // 받은 데미지만큼 체력 감소

        if (currentHp <= 0)
        {
            DestroyObject(); // 체력이 0 이하면 파괴
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            return; // 테스트용 : Player가 아니면 무시
        }

        TakeDamage(1); // 테스트용 : 플레이어가 닿으면 체력 1 감소
    }

    private void DestroyObject()
    {
        if (dropPrefab != null)
        {
            Instantiate(dropPrefab, transform.position, Quaternion.identity); // 파괴 위치에 드랍 오브젝트 생성
        }

        Destroy(gameObject); // 오브젝트 삭제
    }
}
