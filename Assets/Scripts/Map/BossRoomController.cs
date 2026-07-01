using UnityEngine;

public class BossRoomController : MonoBehaviour
{
    [SerializeField] private Door[] doors; // 보스방에서 제어할 문 목록
    [SerializeField] private Transform bossSpawnPoint; // 보스가 생성될 고정 위치

    [Header("보스")]
    [SerializeField] private EnemyBase bossPrefab; // 보스 프리팹
    [SerializeField] private EnemyData bossData; // 생성할 보스 ScriptableObject 데이터

    private EnemyBase spawnedBoss; // 생성된 보스
    private bool roomStarted; // 보스전 시작 여부
    private bool roomCleared; // 보스방 클리어 여부

    public void StartBossRoom()
    {
        if (roomStarted || roomCleared)
        {
            return; // 이미 시작했거나 클리어된 방이면 다시 실행하지 않음
        }

        roomStarted = true;

        CloseDoors(); // 보스방 입장 시 문 닫기
        SpawnBoss(); // 고정 위치에 보스 생성
    }

    private void CloseDoors()
    {
        foreach (Door door in doors)
        {
            if (door != null)
            {
                door.Close(); // 문 닫기
            }
        }
    }

    private void OpenDoors()
    {
        foreach (Door door in doors)
        {
            if (door != null)
            {
                door.Open(); // 문 열기
            }
        }
    }

    private void SpawnBoss()
    {
        if (bossSpawnPoint == null || bossPrefab == null || bossData == null)
        {
            Debug.LogWarning($"{gameObject.name} : BossSpawnPoint 또는 BossPrefab이 연결되지 않았습니다.");
            ClearBossRoom();
            return;
        }

        spawnedBoss = Managers.Pool.GetPool(bossPrefab); // 풀에서 보스 꺼내기
        spawnedBoss.transform.position = bossSpawnPoint.position;
        spawnedBoss.transform.rotation = Quaternion.identity;

        spawnedBoss.Initialize(bossData); // ScriptableObject 데이터로 초기화
        spawnedBoss.OnDead += HandleBossDead; // 보스 사망 이벤트 등록
    }

    private void HandleBossDead(EnemyBase boss)
    {
        boss.OnDead -= HandleBossDead; // 이벤트 중복 방지
        ClearBossRoom(); // 보스 처치 시 방 클리어
    }

    private void ClearBossRoom()
    {
        roomStarted = false; // 전투 진행 상태 종료
        roomCleared = true; // 보스방 클리어 처리

        OpenDoors(); // 보스 처치 후 문 열기

        Debug.Log($"{gameObject.name} 보스 클리어!");
    }
}
