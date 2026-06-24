using UnityEngine;
using UnityEngine.UI;

// 어떤 부위인지 구분하기 위한 이름표
public enum EquipType { None, Helmet, Armor1, Armor2, Boots, Weapon }

public class EquipmentSlot : MonoBehaviour
{
    [Header("슬롯 고유 인덱스 번호")]
    public int slotIndex;

    public Image icon;          // 장착된 장비의 아이콘을 보여줄 Image 컴포넌트
    private ItemData currentItem; // 현재 이 부위에 장착된 아이템 데이터 기억

    private void Start()
    {
        // 장비 부위 버튼을 클릭하면 장착이 해제되도록 설정합니다.
        GetComponent<Button>().onClick.AddListener(OnSlotClick);
    }

    // 장비를 장착했을 때 화면에 아이콘을 띄워주는 함수
    public void DisplayItem(ItemData newItem)
    {
        currentItem = newItem;
        icon.sprite = newItem.itemIcon;
        icon.color = Color.white; // 불투명하게 만들기
    }

    // 장비를 해제했을 때 슬롯을 비우는 함수
    public void ClearSlot()
    {
        currentItem = null;
        icon.sprite = null;
        icon.color = Color.clear; // 투명하게 만들기 (가이드 이미지가 보이게 함)
    }

    // 장착된 장비 슬롯을 클릭했을 때 (장비 해제)
    public void OnSlotClick()
    {
        if (currentItem != null)
        {
            if (EquipmentManager.instance != null)
            {
                Debug.Log($"{gameObject.name} 슬롯 클릭! {slotIndex}번 방 장비를 해제합니다.");

                // 💡 중요: 이제 부위(Enum)가 아니라 '방 번호(int)'를 매니저에게 넘겨줍니다!
                EquipmentManager.instance.Unequip(slotIndex);
            }
        }
    }
}