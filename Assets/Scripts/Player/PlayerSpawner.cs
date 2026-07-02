using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private PlayerBase playerPrefab;

    private void Start()
    {
        if (IsTutorialScene())
        {
            PlayerBase player = Instantiate(playerPrefab, transform.position, Quaternion.identity);
            player.Initialize(playerData);
        }
        else
        {
            PlayerBase player = Managers.Pool.GetPool(playerPrefab);
            player.transform.position = transform.position;
            player.Initialize(playerData);
        }
    }
    private bool IsTutorialScene()
    {
        return UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Stage_Tutorial";
    }
}
