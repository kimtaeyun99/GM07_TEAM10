using UnityEngine;
using UnityEngine.UI;

public class WeaponQuickSlot : MonoBehaviour
{
    [Header("무기 UI 레이아웃 매칭")]
    public Image weapon1Icon; // 💡 5번 방 (현재 손에 든 메인 무기)
    public Image weapon2Icon; // 💡 6번 방 (무기 퀵슬롯 1번)
    public Image weapon3Icon; // 💡 7번 방 (무기 퀵슬롯 2번)

    [Header("빈 슬롯일 때 표시할 기본 스프라이트")]
    public Sprite emptySlotSprite;

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
        // 💡 1키를 누르면 -> 6번 방(퀵1)에 있는 무기를 5번 방(메인)무기와 교체(스왑)
        if (UnityEngine.InputSystem.Keyboard.current != null && UnityEngine.InputSystem.Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            SwapWeaponRooms(6);
        }

        // 💡 2키를 누르면 -> 7번 방(퀵2)에 있는 무기를 5번 방(메인)무기와 교체(스왑)
        if (UnityEngine.InputSystem.Keyboard.current != null && UnityEngine.InputSystem.Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            SwapWeaponRooms(7);
        }
    }

    /// <summary>
    /// 💡 핵심: 5번 방(메인)에 있는 무기와 targetIndex(6번 또는 7번 방)에 있는 무기의 '위치'를 바꿉니다.
    /// </summary>
    private void SwapWeaponRooms(int targetIndex)
    {
        if (EquipmentManager.instance == null) return;

        ItemData[] equipped = EquipmentManager.instance.currentEquipment;

        if (targetIndex >= equipped.Length) return;

        // 퀵슬롯(6번 또는 7번 방)에 무기가 있을 때만 스왑 진행
        if (equipped[targetIndex] != null)
        {
            ItemData tempMain = equipped[5]; // 기존 5번 방(메인) 무기를 잠시 보관

            // 두 방의 데이터를 서로 교체
            equipped[5] = equipped[targetIndex];
            equipped[targetIndex] = tempMain;

            Debug.Log($"[무기 스왑] 5번 방(메인)과 {targetIndex}번 방의 무기를 교체했습니다.");

            // 데이터가 바뀌었으므로 콜백을 실행하여 전반적인 시스템과 UI를 새로고침합니다.
            // (EquipmentManager 내부에 이벤트 인보크 함수가 있다면 그것을 호출하셔도 됩니다)
            RefreshWeaponQuickSlots();
        }
        else
        {
            Debug.LogWarning($"{targetIndex - 5}번 퀵슬롯(장비창 {targetIndex}번 방)이 비어있어 스왑할 수 없습니다.");
        }
    }

    /// <summary>
    /// 5번, 6번, 7번 방의 데이터를 각 UI 슬롯에 정직하게 매칭하여 새로고침합니다.
    /// </summary>
    public void RefreshWeaponQuickSlots()
    {
        if (EquipmentManager.instance == null) return;

        ItemData[] equipped = EquipmentManager.instance.currentEquipment;

        // 1. 5번 방(메인 무기) UI 갱신 및 탄창 동기화
        if (equipped.Length > 5 && equipped[5] != null)
        {
            ItemData mainWeapon = equipped[5];

            if (weapon1Icon != null)
            {
                weapon1Icon.sprite = mainWeapon.itemIcon;
                weapon1Icon.color = Color.white;
            }

            // HUD 탄창에 5번 방 무기의 탄수 동기화
            if (HUDController.instance != null)
            {
                HUDController.instance.SetWeaponAmmo(mainWeapon.currentAmmo, mainWeapon.maxAmmo);
            }
        }
        else
        {
            // 💡 [해결책] 5번 방이 비어있다면 탄창을 0 / 0 으로 만들고 이미지를 비웁니다.
            if (weapon1Icon != null)
            {
                weapon1Icon.sprite = emptySlotSprite;
                weapon1Icon.color = emptySlotSprite != null ? Color.white : Color.clear;
            }

            if (HUDController.instance != null)
            {
                HUDController.instance.SetWeaponAmmo(0, 0);
            }
        }

        // 2. 6번 방(퀵슬롯 1번) UI 갱신
        UpdateSlotUI(equipped, 6, weapon2Icon);

        // 3. 7번 방(퀵슬롯 2번) UI 갱신
        UpdateSlotUI(equipped, 7, weapon3Icon);
    }

    /// <summary>
    /// 각 방 번호에 맞게 순수하게 UI만 그려주는 보조 함수
    /// </summary>
    private void UpdateSlotUI(ItemData[] equippedArray, int targetIndex, Image slotImage)
    {
        if (slotImage == null) return;

        if (targetIndex < equippedArray.Length && equippedArray[targetIndex] != null)
        {
            slotImage.sprite = equippedArray[targetIndex].itemIcon;
            slotImage.color = Color.white;
        }
        else
        {
            if (emptySlotSprite != null)
            {
                slotImage.sprite = emptySlotSprite;
                slotImage.color = Color.white;
            }
            else
            {
                slotImage.sprite = null;
                slotImage.color = Color.clear;
            }
        }
    }
}