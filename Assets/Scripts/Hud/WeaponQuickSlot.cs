using UnityEngine;
using UnityEngine.UI;

public class WeaponQuickSlot : MonoBehaviour
{
    [Header("무기 UI 레이아웃 매칭")]
    public Image pistolSlotIcon;  // 💡 1번 슬롯: 권총 전용 UI
    public Image shotgunSlotIcon; // 💡 2번 슬롯: 샷건 전용 UI
    public Image arSlotIcon;      // 💡 3번 슬롯: AR 전용 UI

    [Header("빈 슬롯일 때 표시할 기본 스프라이트")]
    public Sprite emptySlotSprite;

    [Header("EquipmentManager 방 번호 설정")]
    [Tooltip("현재 캐릭터 손에 들고 있는 메인 무기 방 번호")]
    public int mainWeaponRoomIndex = 5;
    [Tooltip("장비창(또는 가방)에서 무기들을 순서대로 보관하는 시작 방 번호")]
    public int weaponInventoryStartIndex = 6;

    // 내부에서 캐싱하여 사용할 각 무기 슬롯의 장비창 방 번호 (Refresh할 때 실시간 계산됨)
    private int pistolRoomIndex = -1;
    private int shotgunRoomIndex = -1;
    private int arRoomIndex = -1;

    void Start()
    {
        if (EquipmentManager.instance != null)
        {
            EquipmentManager.instance.onEquipmentChangedCallback += RefreshWeaponQuickSlots;
        }

        // 게임 시작 시 초기화
        RefreshWeaponQuickSlots();
    }

    void OnDestroy()
    {
        if (EquipmentManager.instance != null)
        {
            EquipmentManager.instance.onEquipmentChangedCallback -= RefreshWeaponQuickSlots;
        }
    }

    void Update()
    {
        // 💡 숫자 1키 누름 ➡️ 권총 스왑 시도 (장착되어 있을 때만 실행됨)
        if (UnityEngine.InputSystem.Keyboard.current != null && UnityEngine.InputSystem.Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            EquipSelectedWeaponType(pistolRoomIndex, "권총");
        }

        // 💡 숫자 2키 누름 ➡️ 샷건 스왑 시도 (장착되어 있을 때만 실행됨)
        if (UnityEngine.InputSystem.Keyboard.current != null && UnityEngine.InputSystem.Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            EquipSelectedWeaponType(shotgunRoomIndex, "샷건");
        }

        // 💡 숫자 3키 누름 ➡️ AR 스왑 시도 (장착되어 있을 때만 실행됨)
        if (UnityEngine.InputSystem.Keyboard.current != null && UnityEngine.InputSystem.Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            EquipSelectedWeaponType(arRoomIndex, "AR");
        }
    }

    /// <summary>
    /// 💡 핵심: 지정된 타입의 무기 방 번호를 찾아 현재 손(메인 무기)에 들고 있는 무기와 스왑합니다.
    /// </summary>
    private void EquipSelectedWeaponType(int targetRoomIndex, string typeName)
    {
        if (EquipmentManager.instance == null) return;

        ItemData[] equipped = EquipmentManager.instance.currentEquipment;

        // 유효성 체크: 해당 무기 타입이 슬롯에 장착되어 있지 않다면(index가 -1이면) 실행 불가능
        if (targetRoomIndex == -1 || targetRoomIndex >= equipped.Length || equipped[targetRoomIndex] == null)
        {
            Debug.LogWarning($"[퀵슬롯 소지 제한] 현재 소지품/장비창에 {typeName} 무기가 없어 스왑할 수 없습니다.");
            return;
        }

        // 현재 메인 손(5번 방)에 들고 있는 무기와 대상 무기(권총/샷건/AR 중 하나)의 자리를 맞바꿉니다.
        ItemData tempMain = equipped[mainWeaponRoomIndex];
        equipped[mainWeaponRoomIndex] = equipped[targetRoomIndex];
        equipped[targetRoomIndex] = tempMain;

        Debug.Log($"[무기 스왑] 메인 무기를 {typeName}(으)로 교체했습니다.");

        // 데이터 변경에 따른 UI 최신화 호출
        RefreshWeaponQuickSlots();
    }

    /// <summary>
    /// 전체 무기 보관함을 뒤져서 권총, 샷건, AR을 찾아내고 각각의 UI 슬롯을 정직하게 갱신합니다.
    /// </summary>
    public void RefreshWeaponQuickSlots()
    {
        if (EquipmentManager.instance == null) return;

        ItemData[] equipped = EquipmentManager.instance.currentEquipment;

        // 인덱스 초기화 (찾지 못하면 -1로 유지하여 사용 불가 처리)
        pistolRoomIndex = -1;
        shotgunRoomIndex = -1;
        arRoomIndex = -1;

        // 💡 1. 현재 소지하고 있는 전체 장비 리스트(메인 손 포함)를 루프 돌며 무기 종류를 필터링하고 방 번호를 기억합니다.
        // (가방에 무기를 넣었든 장비창 예비 슬롯에 넣었든 상관없이 무기 타입을 실시간 스캔합니다)
        for (int i = 0; i < equipped.Length; i++)
        {
            if (equipped[i] == null) continue;

            string itemName = equipped[i].itemName;

            // 🛠️ 기획하신 무기 데이터 에셋(ItemData)의 Name 구조나 별도의 변수명 규칙에 맞춰서 조건문을 유연하게 매칭해 주세요.
            if (itemName.Contains("권총"))
            {
                pistolRoomIndex = i;
            }
            else if (itemName.Contains("샷건"))
            {
                shotgunRoomIndex = i;
            }
            else if (itemName.Contains("AR") || itemName.Contains("소총"))
            {
                arRoomIndex = i;
            }
        }

        // 💡 2. 메인 손(5번 방) UI 및 탄창 동기화 처리
        if (equipped.Length > mainWeaponRoomIndex && equipped[mainWeaponRoomIndex] != null)
        {
            ItemData mainWeapon = equipped[mainWeaponRoomIndex];
            if (HUDController.instance != null)
            {
                HUDController.instance.SetWeaponAmmo(mainWeapon.currentAmmo, mainWeapon.maxAmmo);
            }
        }
        else
        {
            if (HUDController.instance != null) HUDController.instance.SetWeaponAmmo(0, 0);
        }

        // 💡 3. 수집한 방 번호 결과를 바탕으로 고정형 퀵슬롯 3종 UI 갱신 (소지 중일 때만 불이 들어옴)
        UpdateFixedSlotUI(equipped, pistolRoomIndex, pistolSlotIcon);
        UpdateFixedSlotUI(equipped, shotgunRoomIndex, shotgunSlotIcon);
        UpdateFixedSlotUI(equipped, arRoomIndex, arSlotIcon);

        // 💡 [이 코드를 추가하세요] 5번 방(현재 플레이어 손)에 들린 무기의 진짜 탄약 정보를 HUD에 강제 이식합니다.
        if (EquipmentManager.instance != null && EquipmentManager.instance.currentEquipment.Length > mainWeaponRoomIndex)
        {
            ItemData mainWeapon = EquipmentManager.instance.currentEquipment[mainWeaponRoomIndex];

            if (mainWeapon != null && HUDController.instance != null)
            {
                // HUDController가 엉뚱한 이전 무기의 기억을 가지지 못하도록 진짜 현재 들고 있는 총의 탄약으로 리셋시킵니다.
                HUDController.instance.SetWeaponAmmo(mainWeapon.currentAmmo, mainWeapon.maxAmmo);
                HUDController.instance.UpdateAmmoUI();
            }
        }
    }

    /// <summary>
    /// 고정형 슬롯 전용 UI 새로고침 보조 함수 (아이템 소지 여부에 따라 알파값/스프라이트 제어)
    /// </summary>
    private void UpdateFixedSlotUI(ItemData[] equippedArray, int targetIndex, Image slotImage)
    {
        if (slotImage == null) return;

        // 무기를 소지 중인 상태라면 (방 번호가 유효하고 데이터가 존재할 때)
        if (targetIndex != -1 && targetIndex < equippedArray.Length && equippedArray[targetIndex] != null)
        {
            slotImage.sprite = equippedArray[targetIndex].itemIcon;
            slotImage.color = Color.white; // 불 켜짐 (사용 가능 선명한 상태)
        }
        else
        {
            // 무기가 없을 때 
            if (emptySlotSprite != null)
            {
                slotImage.sprite = emptySlotSprite;
                slotImage.color = Color.white;
            }
            else
            {
                slotImage.sprite = null;
                slotImage.color = new Color(1f, 1f, 1f, 0.2f); // 살짝 반투명하게 숨김 처리하여 빈 슬롯임을 암시
            }
        }
    }
}