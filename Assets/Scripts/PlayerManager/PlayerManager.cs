using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private PlayerCameraController cameraController; // 메인카메라 조절하는 컨트롤러

    public static PlayerManager Instance;
    private bool playerIsLive = true;

   

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }

        cameraController = GetComponent<PlayerCameraController>();
        cameraController.SetTarget(player.transform);   


    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
      
    }
}
