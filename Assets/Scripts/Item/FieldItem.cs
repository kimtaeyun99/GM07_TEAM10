using UnityEngine;

public class FieldItem : MonoBehaviour
{
    [Header("아이템 설정")]
    public ItemData itemData; // 💡 이 필드 아이템이 어떤 아이템 정보를 가졌는지 (ScriptableObject)
    public int count = 1;     // 💡 몇 개짜리 묶음인지 (기본값 1개)

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        // 💡 인스펙터창에 ItemData를 드래그해 두면, 자동으로 그 아이템의 아이콘 모양으로 스프라이트를 변경합니다.
        if (itemData != null && spriteRenderer != null)
        {
            spriteRenderer.sprite = itemData.itemIcon;
        }
    }

    // 💡 플레이어가 아이템을 주웠을 때 오브젝트를 파괴하는 함수
    public void DestroyItem()
    {
        Destroy(gameObject);
    }
}