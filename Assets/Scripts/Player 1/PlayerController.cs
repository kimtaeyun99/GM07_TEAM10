using UnityEngine;

public class PlayerController : MonoBehaviour  // 플레이어의 인풋 ( 마우스 , 키보드를 담당하는 컨트롤러 스크립트)
{

    private PlayerMovement movement;  //플레이어의 움직임을 담당하는 스크립트
    private PlayerGunController gunController; // 플레이어의 총 조작을 담당하는 스크립트
    private PlayerAim aim; // 플래이어의 마우스 커서 위치 정보를 담당하는 스크립트
    private void Awake() // 초기화
    {
        movement = GetComponent<PlayerMovement>();
        gunController = GetComponent<PlayerGunController>();
        aim = GetComponent<PlayerAim>();
    }


    private void Update()
    {
        HandleMovement();
        HandleFire();
        ChangeGun();
    }


    

    private void ChangeGun()
    {
        for(int i = 0; i < PlayerInventory.Instance.QuickSlotLength; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1+i))
            {
                gunController.EquipWeapon(i);
            }
        }
    }


    private void HandleFire() // 무기 관련 조작
    {
        if (Input.GetMouseButton(0))   // 마우스 왼쪽을 눌렀을떄(누르고있을때)
        {
            gunController.Fire(aim.AimDirection);  // PlayerGunController 에서 사격 함수 실행
        }
        if (Input.GetKeyDown(KeyCode.R)) // R키를 눌렀을떄
        {
            gunController.Reload();   // PlayerGunController 에서 재장전 함수 실행 
        }

    }

    private void HandleMovement() // 캐릭터 움직임 관련
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized; // 상하좌우 입력값 vector2 로 저장한 후 대각이동빠름 방지하기위해 normalized
        movement.SetMoveInput(moveInput); // PlayerMovement 에서 움직임 처리

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            movement.TryDodge();
        }
    }


}

