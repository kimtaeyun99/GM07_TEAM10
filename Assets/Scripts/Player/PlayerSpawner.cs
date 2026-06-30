using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private PlayerBase playerPrefab;

    private void Start()
    {
        //PlayerBase player = Instantiate(playerPrefab, transform.position, Quaternion.identity);
        //player.Initialize(playerData);

        PlayerBase player = Managers.Pool.GetPool(playerPrefab);
        player.Initialize(playerData);
    }
}
