using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDConsumableSlot : MonoBehaviour
{
    [Header("소모품 1번 슬롯 UI")]
    public Image slot1Icon;
    public TextMeshProUGUI slot1CountText;

    [Header("소모품 2번 슬롯 UI")]
    public Image slot2Icon;
    public TextMeshProUGUI slot2CountText;

    [Header("빈 슬롯일 때 기본 스프라이트")]
    public Sprite emptySlotSprite;

    void Start()
    {
        if (ConsumableQuickSlot.instance != null)
        {
            ConsumableQuickSlot.instance.onQuickSlotChangedCallback += RefreshHUD;
        }
        RefreshHUD();
    }

    void OnDestroy()
    {
        if (ConsumableQuickSlot.instance != null)
        {
            ConsumableQuickSlot.instance.onQuickSlotChangedCallback -= RefreshHUD;
        }
    }

    public void RefreshHUD()
    {
        if (ConsumableQuickSlot.instance == null) return;

        InventoryItem[] quickItems = ConsumableQuickSlot.instance.quickSlotItems;

        // 1번 소모품 슬롯 갱신 (배열 0번 방)
        UpdateSlotUI(quickItems[0], slot1Icon, slot1CountText, 0);

        // 2번 소모품 슬롯 갱신 (배열 1번 방)
        UpdateSlotUI(quickItems[1], slot2Icon, slot2CountText, 1);
    }

    private void UpdateSlotUI(InventoryItem invItem, Image iconImage, TextMeshProUGUI countText, int slotIndex)
    {
        if (iconImage == null) return;

        // 아이템 정보가 있고 가방(Inventory) 수량이 0보다 클 때만 정상 표시
        if (invItem != null && invItem.itemData != null && invItem.quantity > 0)
        {
            iconImage.sprite = invItem.itemData.itemIcon;
            iconImage.color = Color.white;

            if (countText != null)
            {
                countText.gameObject.SetActive(true);
                countText.text = invItem.quantity.ToString(); // 실시간 수량 연동
            }
        }
        else
        {
            // 가방에서 제거되었거나(0개) 비어있는 상태면 UI 청소
            if (emptySlotSprite != null)
            {
                iconImage.sprite = emptySlotSprite;
                iconImage.color = Color.white;
            }
            else
            {
                iconImage.sprite = null;
                iconImage.color = Color.clear;
            }

            if (countText != null) countText.gameObject.SetActive(false);

            // 데이터 동기화 해제 처리
            if (ConsumableQuickSlot.instance.quickSlotItems[slotIndex] != null)
            {
                ConsumableQuickSlot.instance.quickSlotItems[slotIndex] = null;
            }
        }
    }

    // 💡 인게임 플레이 중 단축키로 퀵슬롯 아이템 사용하기 (예: Q, E 버튼)
    void Update()
    {
        if (UnityEngine.InputSystem.Keyboard.current != null && UnityEngine.InputSystem.Keyboard.current.digit4Key.wasPressedThisFrame) UseQuickSlotItem(0);
        if (UnityEngine.InputSystem.Keyboard.current != null && UnityEngine.InputSystem.Keyboard.current.digit5Key.wasPressedThisFrame) UseQuickSlotItem(1);
    }

    private void UseQuickSlotItem(int slotIndex)
    {
        if (ConsumableQuickSlot.instance == null || Inventory.instance == null) return;

        InventoryItem invItem = ConsumableQuickSlot.instance.quickSlotItems[slotIndex];

        if (invItem != null && invItem.itemData != null && invItem.quantity > 0)
        {
            // 1. 소모품 고유의 효과 실행
            invItem.itemData.Use();

            // 2. 가방 리스트 내부에서 해당 아이템 객체가 들어있는 실제 인덱스를 안전하게 계산
            int realInventoryIndex = Inventory.instance.items.IndexOf(invItem);

            if (realInventoryIndex != -1)
            {
                // 제공해주신 Inventory.cs의 RemoveAt을 그대로 호출해 수량 감소 및 가방 슬롯 파괴 진행
                Inventory.instance.RemoveAt(realInventoryIndex);
            }

            // 3. UI 변경 즉시 반영
            RefreshHUD();
        }
    }
}