using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    [SerializeField] private Transform weaponHolder;
    [SerializeField] private PlayerGunController gunController;
    [SerializeField] private Texture2D aimTexture; // 마우스 커서 텍스쳐 (조준선)
    public Vector2 AimDirection { get; private set; }
    public void Start()
    {
        Vector2 hotSpot = new Vector2(aimTexture.width / 2f, aimTexture.height / 2f); // 조준선 위치보정  /2f 안하면 오른쪽끝 기준으로 설정됨

        Cursor.SetCursor(aimTexture, hotSpot, CursorMode.Auto);
    }

    private void Update()
    {
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);  // 마우스 좌표값을 카메라 기준으로 재계산



        Vector2 mouseScreenPos = Input.mousePosition;   // 마우스좌표값
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos); // 게임월드 좌표값으로 변경
        Vector2 direction = mouseWorldPos - transform.position;   // 오브젝트(총)에서 마우스까지의 거리 계산
        AimDirection = ((Vector2)(mouseWorldPos - transform.position)).normalized; 

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;  // 방향 벡터를 각도로 변경

        if (gunController.CurrentGun != null) // 현재 들고있는 총이 null이면 아래 스프라이트 좌우반전 수행안함
            if (direction.x < 0)              // 총 스프라이트 좌우반전
            {
                gunController.CurrentGun.GunSprite.flipY = true;
            }
            else
            {
                gunController.CurrentGun.GunSprite.flipY = false;
            }

        weaponHolder.rotation = Quaternion.Euler(0, 0, angle);  // 최종 오브젝트 회전



    }
}