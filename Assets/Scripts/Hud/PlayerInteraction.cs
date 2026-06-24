using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    // 💡 자동 습득 방식이므로 Update 함수와 nearbyItem 변수는 더 이상 필요 없습니다.

    // 💡 Trigger Collider 영역 안에 무언가 부딪히는 '순간' 즉시 실행되는 함수
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 부딪힌 오브젝트에 FieldItem 스크립트가 붙어있는지 확인합니다.
        FieldItem item = collision.GetComponent<FieldItem>();

        if (item != null)
        {
            // 💡 닿는 순간 즉시 획득 시도
            AutoPickUpItem(item);
        }
    }

    private void AutoPickUpItem(FieldItem item)
    {
        if (item == null) return;

        bool isAllPickedUp = true;

        // 💡 현재 인벤토리의 Add 메서드가 매개변수를 1개만 받으므로, 아이템 수량만큼 루프를 돌려 추가합니다.
        for (int i = 0; i < item.count; i++)
        {
            bool isPickedUp = Inventory.instance.Add(item.itemData);

            if (!isPickedUp)
            {
                isAllPickedUp = false;
                break; // 가방이 중간에 가득 차면 중단
            }
        }

        if (isAllPickedUp)
        {
            Debug.Log($"[자동 습득] {item.itemData.itemName}을(를) {item.count}개 획득했습니다!");

            // 가방에 성공적으로 다 넣었으므로 필드의 아이템 오브젝트 제거
            item.DestroyItem();
        }
        else
        {
            Debug.LogWarning("가방 공간이 부족하여 아이템을 자동으로 주울 수 없습니다!");
            // 💡 (선택 사항) 만약 가방이 꽉 차서 다 못 주웠다면, 
            // 획득 시도 도중 꽉 차서 남겨진 개수만큼 필드 아이템의 count를 유지할 수도 있습니다.
        }
    }
}