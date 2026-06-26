using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    [SerializeField] private Door[] doors; // 이 방에서 제어할 문 목록
    [SerializeField] private BoxCollider2D spawnArea; // 방 범위 랜덤 생성

    [Header("Normal Enemy")]
    [SerializeField] private EnemyBase[] normalEnemyPrefabs; // 일반 적 프리팹 목록
    [SerializeField] private EnemyData[] normalEnemyDatas; // 일반 적 데이터 목록
    [SerializeField] private int normalEnemyCount = 5; // 일반 적 생성 수

    [Header("Elite Enemy")]
    [SerializeField] private EnemyBase[] eliteEnemyPrefabs; // 엘리트 적 프리팹 목록
    [SerializeField] private EnemyData[] eliteEnemyDatas; // 엘리트 적 데이터 목록
    [SerializeField] private int eliteEnemyCount = 5; // 엘리트 적 생성 수

    private readonly List<EnemyBase> aliveEnemies = new List<EnemyBase>(); // 현재 살아있는 적 목록
    private bool roomStarted; // 방 전투 시작 여부
    private bool roomCleared; // 방 클리어 여부

    public void StartRoom()
    {
        if (roomStarted || roomCleared)
        {
            return; // 이미 시작했거나 클리어된 방이면 다시 실행하지 않음
        }
        roomStarted = true;
        Debug.Log($"{gameObject.name} 전투 시작");

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
                door.Open();
            }
        }
    }

    private void SpawnEnemies()
    {
        if (spawnArea == null)
        {
            Debug.LogWarning($"{gameObject.name} : Enemy Prefab이 연결되지 않았습니다.");
            ClearRoom();
            return;
        }

        SpawnEnemyGroup(normalEnemyPrefabs, normalEnemyDatas, normalEnemyCount); // 일반 적 생성
        SpawnEnemyGroup(eliteEnemyPrefabs, eliteEnemyDatas, eliteEnemyCount); // 엘리트 적 생성

        if (aliveEnemies.Count == 0)
        {
            ClearRoom(); // 생성된 적이 없으면 바로 클리어
        }
    }

    private void SpawnEnemyGroup(EnemyBase[] enemyPrefabs, EnemyData[] enemyDatas, int count)
    {
        if (enemyPrefabs == null || enemyDatas == null)
        {
            return;
        }

        if (enemyPrefabs.Length == 0 || enemyDatas.Length == 0)
        {
            return;
        }

        int usableCount = Mathf.Min(enemyPrefabs.Length, enemyDatas.Length);

        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, usableCount);

            EnemyBase prefab = enemyPrefabs[index];
            EnemyData data = enemyDatas[index];

            SpawnEnemy(prefab, data);
        }
    }

    private void SpawnEnemy(EnemyBase prefab, EnemyData data)
    {
        if (prefab == null || data == null)
        {
            return;
        }
        EnemyBase enemy = Managers.Pool.GetPool(prefab); // 풀에서 적 꺼내기
        enemy.transform.position = GetRandomPosition();
        enemy.transform.rotation = Quaternion.identity;

        enemy.Initialize(data); // ScriptableObject 데이터로 초기화

        aliveEnemies.Add(enemy);
        enemy.OnDead += HandleEnemyDead;
    }

    private Vector2 GetRandomPosition()
    {
        Bounds bound = spawnArea.bounds; // 범위 좌표값
        float x = Random.Range(bound.min.x, bound.max.x); // 범위 안 x 좌표
        float y = Random.Range(bound.min.y, bound.max.y); // 범위 안 y 좌표

        return new Vector2(x, y);
    }

    private void HandleEnemyDead(EnemyBase enemy)
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
