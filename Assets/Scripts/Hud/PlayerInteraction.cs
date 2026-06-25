using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 부딪힌 오브젝트가 필드 아이템인지 확인
        FieldItem item = collision.GetComponent<FieldItem>();

        if (item != null)
        {
            AutoPickUpItem(item);
        }
    }

    private void AutoPickUpItem(FieldItem item)
    {
        if (item == null) return;

        if (item.itemData == null)
        {
            Debug.LogWarning($"{item.gameObject.name}에 ItemData가 연결되어 있지 않습니다!");
            return;
        }

        bool isAllPickedUp = true;
        ItemType type = item.itemData.itemType;

        // 💡 소모품(Consumable)과 장비(Equipment) 모두 '가방(Inventory)'으로 먼저 보냅니다.
        // 장비창(Equip 씬)의 인벤토리 슬롯이 이 데이터와 연동되어 있으므로 실시간으로 가방 칸에 반영됩니다.
        if (type == ItemType.Consumable || type == ItemType.Equipment)
        {
            if (Inventory.instance == null)
            {
                Debug.LogError("Inventory 인스턴스가 씬에 존재하지 않습니다! (가방이 없어 획득 불가)");
                return;
            }

            // 아이템 개수만큼 가방에 차례대로 추가
            for (int i = 0; i < item.count; i++)
            {
                bool isPickedUp = Inventory.instance.Add(item.itemData);
                if (!isPickedUp)
                {
                    isAllPickedUp = false;
                    break; // 가방 공간이 부족하면 루프 중단
                }
            }
        }
        else
        {
            // 그 외 기타 아이템(Etc 등) 처리도 가방으로 전달
            if (Inventory.instance != null)
            {
                Inventory.instance.Add(item.itemData);
            }
        }

        // 💡 성공적으로 인벤토리에 다 담았다면 필드 아이템 오브젝트 제거
        if (isAllPickedUp)
        {
            Debug.Log($"[자동 습득] {item.itemData.itemName}이(가) 인벤토리에 저장되었습니다.");
            item.DestroyItem();
        }
        else
        {
            Debug.LogWarning("인벤토리 가방 공간이 부족하여 아이템을 더 주울 수 없습니다!");
        }
    }
}