using UnityEngine;
using UnityEngine.UI;
using TMPro; // 텍스트 컴포넌트용

public class ConsumableQuickSlot : MonoBehaviour
{
    public static ConsumableQuickSlot instance;

    /// 소모품 퀵슬롯 데이터 2칸 (0번 = 슬롯1, 1번 = 슬롯2)
    public InventoryItem[] quickSlotItems = new InventoryItem[2];

    // 💡 다음번에 아이템이 등록될 슬롯 번호를 기억 (기본값 0 = 1번 슬롯)
    private int nextRegisterIndex = 0;

    // HUD UI 새로고침을 위한 이벤트 콜백
    public delegate void OnQuickSlotChanged();
    public OnQuickSlotChanged onQuickSlotChangedCallback;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 💡 퀵등록 버튼 하나로 호출되는 핵심 순환 함수
    public void RegisterToNextAvailableSlot(InventoryItem invItem)
    {
        // 1. 만약 이미 이 아이템이 다른 슬롯에 등록되어 있다면 중복 등록 방지 (선택 사항)
        if (quickSlotItems[0] == invItem || quickSlotItems[1] == invItem)
        {
            Debug.Log("이미 퀵슬롯에 등록된 아이템입니다.");
            return;
        }

        // 2. 현재 가리키고 있는 빈 곳 또는 순서에 맞게 아이템 등록
        quickSlotItems[nextRegisterIndex] = invItem;
        Debug.Log($"{nextRegisterIndex + 1}번 소모품 퀵슬롯에 {invItem.itemData.itemName} 등록 완료!");

        // 3. 💡 인덱스 순환 로직: 0번 등록했으면 다음엔 1번, 1번 등록했으면 다음엔 다시 0번으로 변경
        nextRegisterIndex = (nextRegisterIndex + 1) % quickSlotItems.Length;

        // HUD에 UI 새로고침 알림 방송
        if (onQuickSlotChangedCallback != null)
        {
            onQuickSlotChangedCallback.Invoke();
        }
    }

    // 가방 데이터가 바뀔 때 수량을 맞추기 위한 리프레시용 함수
    public void RefreshSlots()
    {
        if (onQuickSlotChangedCallback != null)
        {
            onQuickSlotChangedCallback.Invoke();
        }
    }
}
