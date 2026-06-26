using System.Collections.Generic;
using UnityEngine;

// 💡 아이템과 개수를 한 세트로 묶는 클래스
[System.Serializable]
public class InventoryItem
{
    public ItemData itemData;
    public int quantity;

    public InventoryItem(ItemData item, int amt)
    {
        itemData = item;
        quantity = amt;
    }
}

public class Inventory : MonoBehaviour
{
    // 인벤토리 싱글톤 (어디서나 접근 가능하게)
    public static Inventory instance;

    public List<InventoryItem> items = new List<InventoryItem>();

    public int space = 20; // 인벤토리 총 칸수

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    void Awake() => instance = this;

    public bool Add(ItemData item)
    {
        InventoryItem existingItem = items.Find(x => x.itemData == item && x.quantity < item.maxStackSize);

        if (existingItem != null)
        {
            existingItem.quantity++;
            if (onItemChangedCallback != null) onItemChangedCallback.Invoke();

            // 💡 획득 로그 추가 (기존 아이템 수량 증가 시)
            if (HUDLogManager.instance != null)
            {
                HUDLogManager.instance.Log($"[{item.itemName}]을(를) 획득했습니다!", Color.white);
            }
            return true;
        }

        if (items.Count >= space)
        {
            Debug.Log("인벤토리가 가득 찼습니다.");
            return false;
        }

        items.Add(new InventoryItem(item, 1));
        if (onItemChangedCallback != null) onItemChangedCallback.Invoke();

        // 💡 획득 로그 추가 (새로운 슬롯에 등록 시)
        if (HUDLogManager.instance != null)
        {
            HUDLogManager.instance.Log($"[{item.itemName}]을(를) 획득했습니다!", Color.white);
        }
        return true;
    }

    public void RemoveAt(int slotIndex)
    {
        // 안전장치: 방 번호가 범위를 벗어나거나 해당 칸이 비어있으면 취소
        if (slotIndex >= items.Count || items[slotIndex] == null) return;

        // 해당 칸의 수량을 1 줄입니다.
        items[slotIndex].quantity--;

        // 만약 그 칸의 수량이 0개가 되면 가방 리스트에서 해당 칸(슬롯) 자체를 삭제
        if (items[slotIndex].quantity <= 0)
        {
            items.RemoveAt(slotIndex);
        }

        // UI 새로고침 호출
        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
    }
    public int FindIndexByName(string targetName)
    {
        // List의 FindIndex를 사용하여 조건에 맞는 요소의 인덱스를 반환합니다.
        int index = items.FindIndex(x => x.itemData != null && x.itemData.itemName == targetName);

        if (index == -1)
        {
            Debug.LogWarning($"[인벤토리] {targetName} 이라는 이름의 아이템 인덱스를 찾을 수 없습니다.");
        }

        return index;
    }

    public void UseItemByName(string itemName)
    {
        // 1. 이름으로 아이템의 인덱스(방 번호)를 찾습니다.
        int slotIndex = Inventory.instance.FindIndexByName(itemName);

        // 2. ❌ [예외 처리] 아이템이 인벤토리에 없을 경우
        if (slotIndex == -1)
        {
            Debug.LogWarning($"[아이템 사용 실패] 가방에 {itemName}이(가) 없습니다.");

            // HUD 로그 시스템이 있다면 유저에게도 알려줍니다.
            if (HUDLogManager.instance != null)
            {
                HUDLogManager.instance.Log($"[{itemName}]이(가) 부족합니다!", Color.red);
            }

            return; // 💡 여기서 함수를 종료하여 아래 RemoveAt이 실행되지 않도록 막습니다.
        }

        // 3. ⭕ 아이템이 존재할 경우에만 정상적으로 1개 차감
        Inventory.instance.RemoveAt(slotIndex);

        if (HUDLogManager.instance != null)
        {
            HUDLogManager.instance.Log($"[{itemName}]을(를) 사용했습니다.", Color.green);
        }
    }
}