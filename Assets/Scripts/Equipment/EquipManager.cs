using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager instance; // 싱글톤

    // 현재 장착된 아이템들을 담아둘 배열 (0:머리, 1:가슴, 2:무기, 3:신발)
    public ItemData[] currentEquipment = new ItemData[8];

    // 장비창 UI에게 화면을 새로고침하라고 신호를 보낼 이벤트 선언
    public delegate void OnEquipmentChanged();
    public OnEquipmentChanged onEquipmentChangedCallback;

    private void Awake()
    {
        if (instance == null) instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    // EquipmentManager.cs 내부

    public void Equip(ItemData newItem, EquipType targetSlot, int inventoryIndex)
    {
        int slotIndex = (int)targetSlot;

        // 1. 무기 슬롯 3개 우회 분기 로직으로 정확한 방 번호(slotIndex) 결정
        if (targetSlot == EquipType.Weapon)
        {
            if (currentEquipment[5] == null)
            {
                slotIndex = 5;
            }
            else if (currentEquipment[6] == null)
            {
                slotIndex = 6;
            }
            else
            {
                slotIndex = 7;
            }
        }

        // 2. 결정된 장착 슬롯(slotIndex)이 비어있는지 확인
        if (currentEquipment[slotIndex] == null)
        {
            // 💡 [조건 A] 슬롯이 완전히 비어있는 상태일 때
            // 장착 슬롯을 새로 채우는 것이므로, 인벤토리에서 원래 아이템을 제거합니다!
            if (Inventory.instance != null)
            {
                Inventory.instance.RemoveAt(inventoryIndex);
                Debug.Log($"{newItem.itemName}을 빈 슬롯에 장착하여 인벤토리에서 차감했습니다.");
            }

            // 새 아이템 장착
            currentEquipment[slotIndex] = newItem;
        }
        else
        {
            // 💡 [조건 B] 슬롯이 이미 차있어서 교체(Swap)해야 할 때
            // 인벤토리에서 제거(RemoveAt)하지 않고, 그 칸의 내용을 기존 아이템과 서로 교체합니다.
            ItemData oldItem = currentEquipment[slotIndex];

            // 장착 슬롯에 새 아이템 덮어씌우기
            currentEquipment[slotIndex] = newItem;

            if (Inventory.instance != null)
            {
                // 💡 인벤토리의 원래 그 칸(inventoryIndex)에 기존 장비를 다시 대입하여 바꿔치기합니다.
                // (이 기능이 정상 작동하려면 Inventory에 인덱스로 아이템을 세팅하는 기능이 있거나, 
                //  단순 Add 후 UI 새로고침을 통해 자연스럽게 Swap 연출을 해야 합니다)

                // 예시: 가장 안전하게 기존 아이템을 인벤토리에 다시 대입하는 방식
                // 만약 Inventory.cs에 items[inventoryIndex]를 수정하는 함수가 없다면 아래처럼 직접 접근하거나 Add를 활용합니다.
                Inventory.instance.items[inventoryIndex] = new InventoryItem(oldItem, 1);

                Debug.Log($"기존 장비 {oldItem.itemName}과 새 장비 {newItem.itemName}을 교체(Swap)했습니다.");
            }
        }

        // UI 새로고침 호출
        if (onEquipmentChangedCallback != null)
        {
            onEquipmentChangedCallback.Invoke();
        }
    }

    // 💡 기존의 해제 함수 (이름은 프로젝트에 따라 Unequip, RemoveEquipment 등 다를 수 있습니다)
    public void Unequip(int slotIndex)
    {
        // 안전장치: 방 번호가 범위를 벗어나거나, 해당 칸이 이미 비어있다면 취소
        if (slotIndex >= currentEquipment.Length || currentEquipment[slotIndex] == null) return;

        // 해제할 아이템을 임시로 기억해 둡니다.
        ItemData unequippedItem = currentEquipment[slotIndex];

        // 💡 핵심: 해당 방을 비워줍니다 (주무기면 4번, 보조무기면 6번 방이 정확히 비워집니다)
        currentEquipment[slotIndex] = null;

        // 해제한 아이템을 플레이어의 인벤토리 가방에 다시 넣어줍니다.
        if (Inventory.instance != null)
        {
            Inventory.instance.Add(unequippedItem);
        }

        // UI를 새로고침하는 콜백 함수 호출
        if (onEquipmentChangedCallback != null)
        {
            onEquipmentChangedCallback.Invoke();
        }
    }
}