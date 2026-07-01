using UnityEngine;

public class InteractableBox : MonoBehaviour
{
    [Header("상자 이미지 설정")]
    [SerializeField] private Sprite openedBoxSprite; // 열린 상자 이미지

    [Header("보상 골드 설정 (Direct Input)")]
    [SerializeField] private int rewardGold = 100; // 💡 인스펙터에서 지급할 골드 액수 설정 가능

    [Header("보상 아이템 설정 (ItemData 에셋 등록)")]
    [SerializeField] private ItemData potionItemData; // 💡 포션의 ItemData
    [SerializeField] private ItemData ammoItem1Data;   // 💡 탄창의 ItemData
    [SerializeField] private ItemData ammoItem2Data;   // 💡 탄창의 ItemData

    private SpriteRenderer spriteRenderer;
    private bool isOpened = false; //
    private bool isPlayerInside = false; // 플레이어가 상자 영역 안에 들어와 있는지 여부

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); //
    }

    private void Update()
    {
        // 이미 열렸다면 감시하지 않음
        if (isOpened) return;

        // 플레이어가 충돌 영역 안에 있고 + E키(상호작용)가 눌렸다면 상호작용 실행
        if (isPlayerInside && Managers.Input.isInteractPressed)
        {
            Interact();
            Managers.Input.isInteractPressed = false; // 입력 소모 (연속 입력 방지)
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 부딪힌 오브젝트의 컴포넌트를 통해 플레이어인지 확인
        if (other.GetComponent<PlayerBase>() != null)
        {
            isPlayerInside = true;
            Debug.Log("[상자] 플레이어 진입! E키를 누르면 상자가 열립니다.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<PlayerBase>() != null)
        {
            isPlayerInside = false;
            Debug.Log("[상자] 플레이어가 범위를 벗어났습니다.");
        }
    }

    public void Interact()
    {
        if (isOpened) return; // 이미 열려있다면 무시

        // 1. 인벤토리 인스턴스가 존재하는지 안전하게 체크
        if (Inventory.instance == null)
        {
            Debug.LogError("Inventory 인스턴스가 씬에 없습니다! 보상을 지급할 수 없습니다.");
            return;
        }

        isOpened = true; //

        // 2. 이미지를 열린 상자로 교체
        if (openedBoxSprite != null && spriteRenderer != null)
        {
            spriteRenderer.sprite = openedBoxSprite;
        }

        // 3. 💡 [추가] 골드 보상 지급
        if (rewardGold > 0)
        {
            Inventory.instance.gold += rewardGold; // 인벤토리의 골드 프로퍼티에 합산

            // 만약 화면에 획득 텍스트 로그를 띄워주는 시스템이 있다면 골드 로그도 추가 가능합니다.
            if (HUDLogManager.instance != null)
            {
                HUDLogManager.instance.Log($"{rewardGold:N0} 골드를 획득했습니다!", Color.yellow);
            }
            Debug.Log($"[상자 보상] {rewardGold} 골드 지급 완료.");
        }

        // 4. 포션 5개 지급
        if (potionItemData != null)
        {
            for (int i = 0; i < 5; i++)
            {
                bool success = Inventory.instance.Add(potionItemData); //
                if (!success) { Debug.LogWarning("인벤토리가 가득 차서 포션을 더 넣을 수 없습니다!"); break; }
            }
        }

        // 5. 탄창 5개 지급
        if (ammoItem1Data != null)
        {
            for (int i = 0; i < 5; i++)
            {
                bool success = Inventory.instance.Add(ammoItem1Data); //
                if (!success) { Debug.LogWarning("인벤토리가 가득 차서 탄창을 더 넣을 수 없습니다!"); break; }
            }
        }

        if (ammoItem2Data != null)
        {
            for (int i = 0; i < 5; i++)
            {
                bool success = Inventory.instance.Add(ammoItem2Data); //
                if (!success) { Debug.LogWarning("인벤토리가 가득 차서 탄창을 더 넣을 수 없습니다!"); break; }
            }
        }

        Debug.Log($"[상자 보상] 모든 보상 지급 완료.");
    }
}