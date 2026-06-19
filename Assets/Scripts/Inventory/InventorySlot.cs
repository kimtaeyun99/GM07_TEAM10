using UnityEngine;
using UnityEngine.UI;
using TMPro; // 💡 TextMeshPro를 사용하신다면 추가, 일반 Text면 생략

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public TMP_Text quantityText;

    // 💡 추가: 이 슬롯이 인벤토리 리스트에서 몇 번째 칸인지 저장할 변수
    private int myIndex;
    private InventoryItem currentSlotItem;

    // 💡 변경: AddItem을 할 때 자신이 몇 번째 방인지 index 번호도 같이 받아서 저장합니다.
    public void AddItem(InventoryItem newItem, int index)
    {
        currentSlotItem = newItem;
        myIndex = index; // 💡 내 방 번호 저장 (예: 0번 칸, 1번 칸...)

        icon.sprite = newItem.itemData.itemIcon;
        icon.color = Color.white;

        if (newItem.itemData.itemType == ItemType.Consumable && newItem.quantity >= 1)
        {
            quantityText.gameObject.SetActive(true);
            quantityText.text = $"{newItem.quantity} / {newItem.itemData.maxStackSize}";
        }
        else
        {
            quantityText.gameObject.SetActive(false);
        }
    }

    public void OnSlotClick()
    {
        if (currentSlotItem != null && InventoryUI.instance != null)
        {
            // 💡 중요: 설명창을 띄울 때 내 데이터(currentSlotItem)와 내 방 번호(myIndex)를 함께 보냅니다!
            InventoryUI.instance.ShowDescription(currentSlotItem, myIndex);
        }
    }

    // 슬롯을 깨끗하게 비우는 함수
    public void ClearSlot()
    {
        currentSlotItem = null;
        icon.sprite = null;
        icon.color = Color.clear;

        // 💡 슬롯이 비어있을 때는 무조건 텍스트창을 비활성화합니다.
        if (quantityText != null)
        {
            quantityText.gameObject.SetActive(false);
        }
    }

}