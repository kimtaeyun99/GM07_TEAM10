using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private PlayerBase player;
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);

    private void Start()
    {
        if (player == null)
        {
            player = FindAnyObjectByType<PlayerBase>();
        }
    }
    private void Update()
    {
        if (player == null) return;
        Vector3 targetPos = player.transform.position + offset;
        transform.position = targetPos;
    }
}
