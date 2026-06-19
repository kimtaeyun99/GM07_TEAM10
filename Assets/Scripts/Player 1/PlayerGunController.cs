using Unity.VisualScripting;
using UnityEngine;

public class PlayerGunController : MonoBehaviour
{
    [SerializeField] private Transform weaponHolder; // 총의 위치값
    [SerializeField] private Gun currentGun;  // 현재 사용중인(착용한) 무기

    public Gun CurrentGun => currentGun;  // 외부에서 currentGun을 쓸수있게
    private void Start()
    {
        if (currentGun == null)      // 초반 인벤토리에 무기가 있으면 맨앞에있는걸 장착
        {
            PlayerInventory inventory = PlayerInventory.Instance;
            for(int i =0; i < inventory.QuickSlotLength; i++)
            {
                if(inventory.GetQuickSlot(i) != null)
                {
                    currentGun = inventory.GetQuickSlot(i);
                    currentGun.gameObject.SetActive(true);
                    break;
                }
            }
        }
    }


    // EquipWeapon : 현재 사용할 무기 설정하는 메소드 Gun 타입 인자는 현재 사용안함!
    public void EquipWeapon(Gun weaponPrefab) // 무기프리팹으로 설정시
    {
        if(weaponPrefab == currentGun)  // 같은 무기일시 리턴
        {
            Debug.Log("같은무기");
            return;
        }

        if (currentGun != null)    // 현재 사용중인 무기가 있으면
        {
            //Destroy(currentGun.gameObject);  // 오브젝트 삭제
        }

        currentGun = Instantiate(weaponPrefab, weaponHolder.position, weaponHolder.rotation, weaponHolder); // 새 무기 프리팹 생성 }
    }
    public void EquipWeapon(int index) // 무기장착 함수 퀵슬롯 index로 접근
    {   

        if (currentGun.IsSwap)
        {
            return;
        }
        Gun quickSlotGun = PlayerInventory.Instance.GetQuickSlot(index);
        if(quickSlotGun == null)   // 퀵슬롯이 비어있으면 리턴
        {
            Debug.Log("해당 큇슬롯에 아무것도 없습니다");
            return;
        }
        if (quickSlotGun == currentGun)  // 같은 무기일시 리턴
        {
            Debug.Log("같은무기");
            return;
        }
        currentGun.gameObject.SetActive(false);  // 기존 무기프리팹 비황성화
        currentGun = quickSlotGun;               // 장착 무기 변경
        currentGun.gameObject.SetActive(true);   // 장착 무기 활성화
        PlayerInventory.Instance.ChangeCurrentSlot(index);  // 퀵슬롯에 현재 선택된 인덱스 갱신
    }

    public void Fire(Vector2 direction) // 사격
    {
        if (currentGun == null)
            return;

        currentGun.TryFire(direction);   // Gun 프리팹의 사격로직 실행
    }

    public void Reload() // 재장전
    {
        if (currentGun == null)
            return;

        currentGun.TryReload(); // Gun 프리팹의 재장전로직 실행
    }
}