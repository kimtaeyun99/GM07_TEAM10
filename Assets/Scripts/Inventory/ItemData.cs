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

    [Header("소비템 회복량 설정")]
    public int healAmount = 0;   // 💡 회복량 변수 추가

    [Header("무기(Weapon) 타입일 때만 설정하는 탄창")]
    public int maxAmmo = 0;
    public int currentAmmo = 0;

    // ItemData.cs 내부 추가 예시
    [Header("장비 스탯 설정")]
    public int bonusMaxHp;     // 이 장비를 끼면 오르는 최대 체력
    public float bonusSpeed;   // 이 장비를 끼면 오르는 이동 속도

    public void Use()
    {
        if (itemName.Contains("포션"))
        {
            PlayerBase player = FindAnyObjectByType<PlayerBase>();
            if (player != null)
            {
                // 이미 만피면 사용 취소 (false 반환)
                if (player.currentHp >= player.maxHp)
                {
                    if (HUDLogManager.instance != null) HUDLogManager.instance.Log("체력이 이미 가득 차 있습니다!", Color.yellow);
                    return;
                }

                // 회복 및 최대치 고정
                player.currentHp = Mathf.Min(player.currentHp + healAmount, player.maxHp);

                // 플레이어 체력 이벤트 호출 대신 직접 HUD 알림 (PlayerBase 구조에 연동)
                player.TakeDamage(0); // TakeDamage(0)을 호출하면 내부에서 OnHealthChanged 델리게이트가 정상 호출되어 UI가 갱신됩니다.

                Debug.Log($"{itemName} 사용함. 체력 회복: {healAmount}");
                return; // 성공적으로 아이템을 소비함
            }
        }

        Debug.Log(itemName + "을 사용했으나 효과가 없습니다."); //
        return;
    }
}