using UnityEngine;
using UnityEngine.UI;

public class WeaponQuickSlot : MonoBehaviour
{
    public static WeaponQuickSlot instance;

    [Header("무기 UI 레이아웃 매칭 (제자리 고정)")]
    public Image pistolIcon;   // 1번 슬롯 UI (5번 장비창 매칭)
    public Image shotgunIcon;  // 2번 슬롯 UI (6번 장비창 매칭)
    public Image arIcon;       // 3번 슬롯 UI (7번 장비창 매칭)

    [Header("빈 슬롯일 때 표시할 기본 스프라이트")]
    public Sprite emptySlotSprite;

    [Header("선택된 슬롯 시각 효과")]
    public Color selectedColor = Color.white;
    public Color unselectedColor = new Color(0.5f, 0.5f, 0.5f, 0.6f);

    // 현재 키보드로 선택해 활성화된 퀵슬롯 번호 (-1: 맨손, 0: 권총, 1: 샷건, 2: AR)
    private int activePrefabIndex = -1;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (EquipmentManager.instance != null)
        {
            EquipmentManager.instance.onEquipmentChangedCallback += RefreshWeaponQuickSlots;
        }
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
        if (UnityEngine.InputSystem.Keyboard.current == null) return;

        // 💡 1, 2, 3키 입력 시 장비창 해당 슬롯에 아이템이 실존하는지만 다이렉트로 검사
        if (UnityEngine.InputSystem.Keyboard.current.digit1Key.wasPressedThisFrame) TrySelectWeapon(0, 5);
        if (UnityEngine.InputSystem.Keyboard.current.digit2Key.wasPressedThisFrame) TrySelectWeapon(1, 6);
        if (UnityEngine.InputSystem.Keyboard.current.digit3Key.wasPressedThisFrame) TrySelectWeapon(2, 7);
    }

    /// <summary>
    /// 지정된 장비창 슬롯(equipmentSlot)에 무기가 장착되어 있는지 확인하고 활성화 상태를 변경합니다.
    /// </summary>
    private void TrySelectWeapon(int quickSlotIdx, int equipmentSlot)
    {
        if (EquipmentManager.instance == null || EquipmentManager.instance.currentEquipment == null) return;

        // 문자열이나 프리팹 비교 없이, 해당 인벤토리 장비칸에 ItemData가 채워져 있는지만 확인!
        bool isEquipped = EquipmentManager.instance.currentEquipment[equipmentSlot] != null;

        if (isEquipped)
        {
            activePrefabIndex = quickSlotIdx;
            Debug.Log($"[퀵슬롯 상태 업데이트] {quickSlotIdx + 1}번 무기 선택 가능 상태로 변경.");
        }
        else
        {
            Debug.LogWarning($"[무기 선택 불가] 인벤토리 장비창 {equipmentSlot}번 슬롯에 무기가 장착되어 있지 않습니다.");
        }

        RefreshWeaponQuickSlots();
    }

    /// <summary>
    /// 장비창 상태에 맞춰 아이콘 스프라이트만 동적으로 가져와 제자리에 고정 출력합니다.
    /// </summary>
    public void RefreshWeaponQuickSlots()
    {
        if (EquipmentManager.instance == null || EquipmentManager.instance.currentEquipment == null) return;
        ItemData[] equipped = EquipmentManager.instance.currentEquipment;

        // 각 무기 슬롯(5, 6, 7)에 아이템이 있다면 아이콘 장착, 없다면 empty 기본 스프라이트 적용
        UpdateSlotUI(pistolIcon, equipped[5], activePrefabIndex == 0);
        UpdateSlotUI(shotgunIcon, equipped[6], activePrefabIndex == 1);
        UpdateSlotUI(arIcon, equipped[7], activePrefabIndex == 2);

        // 손에 들고 있던 슬롯의 아이템이 해제(Null)되었다면 선택 상태도 맨손(-1)으로 리셋
        if (activePrefabIndex != -1)
        {
            int checkSlot = activePrefabIndex == 0 ? 5 : (activePrefabIndex == 1 ? 6 : 7);
            if (equipped[checkSlot] == null) activePrefabIndex = -1;
        }
    }

    private void UpdateSlotUI(Image slotImage, ItemData item, bool isSelected)
    {
        if (slotImage == null) return;

        // 아이템이 있다면 아이콘 이미지 적용, 없다면 빈 슬롯 스프라이트 적용
        slotImage.sprite = (item != null) ? item.itemIcon : emptySlotSprite;
        slotImage.color = isSelected ? selectedColor : unselectedColor;
    }

    public int GetActivePrefabIndex()
    {
        return activePrefabIndex;
    }
}