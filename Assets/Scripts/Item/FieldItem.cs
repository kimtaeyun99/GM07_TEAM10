using UnityEngine;

public class FieldItem : MonoBehaviour
{
    [Header("아이템 설정")]
    public ItemData itemData; // ScriptableObject 정보
    public int count = 1;     // 💡 아이템 개수 (골드일 경우 '골드 수량')

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        // 인스펙터창에 연결된 ItemData의 아이콘으로 외형 자동 설정
        if (itemData != null && spriteRenderer != null)
        {
            spriteRenderer.sprite = itemData.itemIcon;
        }
    }

    /// <summary>
    /// 💡 플레이어가 아이템 또는 골드 상자와 충돌하거나 획득 키를 눌렀을 때 실행할 메인 함수
    /// </summary>
    public void OnAcquire()
    {
        if (itemData == null || Inventory.instance == null) return;

        // 1. 아이템 타입이 '골드'인 경우 인벤토리의 골드 잔액을 직접 올려줍니다.
        if (itemData.itemType == ItemType.Gold)
        {
            Inventory.instance.gold += count;

            if (HUDLogManager.instance != null)
            {
                HUDLogManager.instance.Log($"[{count} 골드]를 획득했습니다!", Color.yellow);
            }
        }
        else // 2. 일반 아이템(소모품, 장비 등)인 경우 가방 리스트에 보관합니다.
        {
            // 기존 Inventory.Add는 1개씩 추가되도록 설계되어 있으므로 count만큼 반복 추가 처리
            for (int i = 0; i < count; i++)
            {
                Inventory.instance.Add(itemData);
            }
        }

        DestroyItem();
    }

    public void DestroyItem()
    {
        Destroy(gameObject);
    }

    // 💡 지나가다 닿기만 해도 획득(자동 루팅)되게 하고 싶다면 아래 트리거를 활용합니다.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerBase>() != null)
        {
            OnAcquire();
        }
    }
}