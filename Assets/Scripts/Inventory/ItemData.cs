using UnityEngine;

// 1. 아이템 종류 구분용 이름표 (💡 Gold 추가)
public enum ItemType { Consumable, Equipment, Etc, Gold }

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;       // 아이템 이름
    public Sprite itemIcon;
    public int itemPrice;

    [TextArea]
    public string description;    // 아이템 설명

    [Header("아이템 중첩 설정")]
    public int maxStackSize = 5;

    [Header("아이템 타입 설정")]
    public ItemType itemType;     // 💡 기획자가 Gold를 선택할 수 있게 됨

    [Header("장비일 때만 설정하는 부위")]
    public EquipType targetEquipSlot;

    [Header("무기(Weapon) 타입일 때만 설정하는 탄창")]
    public int maxAmmo = 0;
    public int currentAmmo = 0;

    public void Use()
    {
        Debug.Log(itemName + "을 사용했습니다.");
    }
}