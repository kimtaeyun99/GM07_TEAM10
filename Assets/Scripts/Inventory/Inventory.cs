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
}