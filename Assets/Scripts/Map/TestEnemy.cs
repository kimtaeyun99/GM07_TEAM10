using System;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    public event Action<TestEnemy> OnDead; // 적이 죽었을 때 RoomController에 알림

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return; // Player가 아니면 무시
        }

        Die(); // 테스트용 : 플레이어가 닿으면 적 처치
    }

    private void Die()
    {
        OnDead?.Invoke(this); // RoomController에 사망 알림
        Destroy(gameObject); // 적 오브젝트 삭제
    }
}
