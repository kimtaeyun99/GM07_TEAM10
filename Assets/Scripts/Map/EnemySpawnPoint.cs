using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    [SerializeField] private string spawnId;
    [SerializeField] private EnemySpawnType spawnType;

    public string SpawnId
    {
        get { return spawnId; }
    }
    public EnemySpawnType SpawnType
    {
        get { return spawnType; }
    }
}

public enum EnemySpawnType
{
    Normal,
    Ranged,
    Melee,
    Elite,
    BossMinion
}
