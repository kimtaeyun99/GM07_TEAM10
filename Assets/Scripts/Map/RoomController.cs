using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    [SerializeField] private Door[] doors; // 이 방에서 제어할 문 목록
    [SerializeField] private EnemySpawnPoint[] spawnPoints; // 적 생성 위치 목록
    [SerializeField] private BoxCollider2D spawnArea; // 방 범위 랜덤 생성
    [SerializeField] private int enemyCount;

    [SerializeField] private GameObject enemyPrefab; // 테스트용 적 프리팹

    private readonly List<TestEnemy> aliveEnemies = new List<TestEnemy>(); // 현재 살아있는 적 목록
    private bool roomStarted; // 방 전투 시작 여부
    private bool roomCleared; // 방 클리어 여부

    public void StartRoom()
    {
        if (roomStarted || roomCleared)
        {
            return; // 이미 시작했거나 클리어된 방이면 다시 실행하지 않음
        }
        roomStarted = true;
        Debug.Log("Start Room");

        CloseDoors(); // 방 입장 시 문 닫기
        SpawnEnemies(); // SpawnPoint 위치에 적 생성
    }

    private void CloseDoors()
    {
        foreach (Door door in doors)
        {
            if (door != null)
            {
                door.Close();
            }
        }
    }

    private void OpenDoors()
    {
        foreach (Door door in doors)
        {
            if (door != null)
            {
                door.Open(); // 연결된 문 열기
            }
        }
    }

    private void SpawnEnemies()
    {
        if (enemyPrefab == null)
        {
            Debug.LogWarning($"{gameObject.name} : Enemy Prefab이 연결되지 않았습니다.");
            ClearRoom();
            return;
        }

        for (int i = 0; i < enemyCount; i++)
        {

            GameObject enemyObject = Instantiate(
                enemyPrefab,
                GetRandomPosition(),
                Quaternion.identity
                );
            TestEnemy enemy = enemyObject.GetComponent<TestEnemy>();

            if (enemy != null)
            {
                aliveEnemies.Add(enemy);
                enemy.OnDead += HandleEnemyDead; // 적 사망 이벤트 등록
            }
        }

        if (aliveEnemies.Count == 0)
        {
            ClearRoom(); // 생성된 적이 없으면 바로 클리어
        }
    }

    private Vector2 GetRandomPosition()
    {
        Bounds bound = spawnArea.bounds; // 범위 좌표값
        float x = Random.Range(bound.min.x, bound.max.x);
        float y = Random.Range(bound.min.y, bound.max.y);
        return new Vector2(x, y);
    }

    private void HandleEnemyDead(TestEnemy enemy)
    {
        enemy.OnDead -= HandleEnemyDead; // 이벤트 중복 방지
        aliveEnemies.Remove(enemy); // 죽은 적 목록에서 제거

        if (aliveEnemies.Count <= 0)
        {
            ClearRoom(); // 모든 적 처치 시 방 클리어
        }
    }

    private void ClearRoom()
    {
        roomStarted = false; // 전투 진행 상태 종료
        roomCleared = true; // 방 클리어 처리

        OpenDoors(); // 방 클리어 시 문 열기

        Debug.Log($"{gameObject.name} 클리어");
    }
}
