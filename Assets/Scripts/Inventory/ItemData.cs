using UnityEngine;

// 1. 아이템 종류 구분용 이름표
public enum ItemType { Consumable, Equipment, Etc }

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;       // 아이템 이름
    public Sprite itemIcon;       // 인벤토리에 표시될 아이콘
    [TextArea]
    public string description;    // 아이템 설명

    [Header("아이템 중첩 설정")]
    // 💡 슬롯 하나에 최대 몇 개까지 겹칠 수 있는지 설정 (기본값 5)
    public int maxStackSize = 5;

    [Header("아이템 타입 설정")]
    public ItemType itemType;     // 소모품인지 장비인지 기획자가 인스펙터에서 선택

    [Header("장비일 때만 설정하는 부위")]
    // 💡 아까 EquipmentSlot.cs에서 만든 EquipType을 그대로 사용합니다.
    public EquipType targetEquipSlot;

    // 아이템 사용 효과 (기존 함수)
    public void Use()
    {
        // 소모품일 때의 로직 (예: 체력 회복 등)
        Debug.Log(itemName + "을 사용했습니다.");
    }
}