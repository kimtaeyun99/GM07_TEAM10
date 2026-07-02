using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private PlayerBase playerPrefab;

    private void Start()
    {
        PlayerBase player = Managers.Pool.GetPool(playerPrefab);
        player.transform.position = transform.position;
        player.Initialize(playerData);
    }
}
