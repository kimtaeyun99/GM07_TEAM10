using UnityEngine;
using UnityEngine.UI;

public class WeaponQuickSlot : MonoBehaviour
{
    public static WeaponQuickSlot instance;

    [Header("🔥 다른 사람이 만든 무기 프리팹 직접 등록")]
    // 💡 유니티 인스펙터창에서 다른 사람이 만든 Pistol, Shotgun, AR 프리팹을 직접 넣어줍니다.
    public WeaponBase pistolPrefab;
    public WeaponBase shotgunPrefab;
    public WeaponBase arPrefab;

    [Header("무기 UI 레이아웃 매칭 (제자리 고정)")]
    public Image pistolIcon;   // 1번 슬롯 UI
    public Image shotgunIcon;  // 2번 슬롯 UI
    public Image arIcon;       // 3번 슬롯 UI

    [Header("빈 슬롯일 때 표시할 기본 스프라이트")]
    public Sprite emptySlotSprite;

    [Header("선택된 슬롯 시각 효과")]
    public Color selectedColor = Color.white;
    public Color unselectedColor = new Color(0.5f, 0.5f, 0.5f, 0.6f);

    // 현재 키보드로 선택해 손에 든 무기 고정 번호 (-1: 맨손, 0: 권총, 1: 샷건, 2: AR)
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

        // 1, 2, 3키 입력 시 각 무기 타입이 인벤토리에 있는지 검사 후 활성화
        if (UnityEngine.InputSystem.Keyboard.current.digit1Key.wasPressedThisFrame) TrySelectWeapon(0, typeof(WeaponBase)); // (권총 스크립트 클래스명이 있다면 바꿀 수 있음)
        if (UnityEngine.InputSystem.Keyboard.current.digit2Key.wasPressedThisFrame) TrySelectWeapon(1, typeof(Shotgun));
        if (UnityEngine.InputSystem.Keyboard.current.digit3Key.wasPressedThisFrame) TrySelectWeapon(2, typeof(AR));
    }

    /// <summary>
    /// ItemData 내부가 아닌, 시스템에 등록된 무기 컴포넌트 타입을 비교하여 장착 유무를 확인합니다.
    /// </summary>
    private void TrySelectWeapon(int prefabIndex, System.Type weaponComponentType)
    {
        if (EquipmentManager.instance == null || EquipmentManager.instance.currentEquipment == null) return;

        ItemData[] equipped = EquipmentManager.instance.currentEquipment;
        bool isEquipped = false;

        // 장비창 무기 슬롯(5, 6, 7번 방) 검사
        for (int i = 5; i <= 7; i++)
        {
            if (equipped[i] != null)
            {
                // 💡 힌트: 아이템 데이터 이름에 해당 키워드가 묻어있거나 일치하는지 최소한의 안전장치로 판별
                string nameLower = equipped[i].itemName.ToLower();
                if (weaponComponentType == typeof(Shotgun) && (nameLower.Contains("shotgun") || nameLower.Contains("shogun") || nameLower.Contains("샷건"))) isEquipped = true;
                else if (weaponComponentType == typeof(AR) && (nameLower.Contains("ar") || nameLower.Contains("소총"))) isEquipped = true;
                else if (prefabIndex == 0 && (nameLower.Contains("pistol") || nameLower.Contains("권총"))) isEquipped = true;
            }
        }

        if (isEquipped)
        {
            activePrefabIndex = prefabIndex;
            Debug.Log($"[무기 활성화] {weaponComponentType.Name} 무기를 꺼냈습니다.");
        }
        else
        {
            Debug.LogWarning($"[무기 선택 불가] 장비창에 {weaponComponentType.Name} 계열의 무기가 장착되어 있지 않습니다.");
        }

        RefreshWeaponQuickSlots();
    }

    public void RefreshWeaponQuickSlots()
    {
        if (EquipmentManager.instance == null || EquipmentManager.instance.currentEquipment == null) return;
        ItemData[] equipped = EquipmentManager.instance.currentEquipment;

        ResetIcons();

        // 장착된 ItemData들의 이름을 기반으로 알맞은 퀵슬롯 UI 칸에 아이콘을 강제 매칭
        for (int i = 5; i <= 7; i++)
        {
            ItemData item = equipped[i];
            if (item == null) continue;

            string nameLower = item.itemName.ToLower();

            if (nameLower.Contains("pistol") || nameLower.Contains("권총"))
                SetSlotUI(pistolIcon, item.itemIcon, activePrefabIndex == 0);
            else if (nameLower.Contains("shotgun") || nameLower.Contains("shogun") || nameLower.Contains("샷건"))
                SetSlotUI(shotgunIcon, item.itemIcon, activePrefabIndex == 1);
            else if (nameLower.Contains("ar") || nameLower.Contains("소총"))
                SetSlotUI(arIcon, item.itemIcon, activePrefabIndex == 2);
        }
    }

    private void SetSlotUI(Image slotImage, Sprite iconSprite, bool isSelected)
    {
        if (slotImage == null) return;
        slotImage.sprite = iconSprite;
        slotImage.color = isSelected ? selectedColor : unselectedColor;
    }

    private void ResetIcons()
    {
        SetSlotUI(pistolIcon, emptySlotSprite, activePrefabIndex == 0);
        SetSlotUI(shotgunIcon, emptySlotSprite, activePrefabIndex == 1);
        SetSlotUI(arIcon, emptySlotSprite, activePrefabIndex == 2);
    }

    public int GetActivePrefabIndex()
    {
        return activePrefabIndex;
    }
}