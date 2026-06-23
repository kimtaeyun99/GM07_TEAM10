using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private PlayerBase playerPrefab;

    private void Start()
    {
        PlayerBase player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        player.Initialize(playerData);
    }
}
