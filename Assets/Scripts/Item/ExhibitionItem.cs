using UnityEngine;

public class ExhibitionItem : MonoBehaviour
{
    [Header("Item Configuration")]
    [SerializeField] private ItemData itemData; // 💡 여기서 가질 ItemData

    private bool isPlayerInside = false; // 플레이어가 아이템 영역 안에 들어와 있는지 여부

    private void Update()
    {
        // 1. 이미 아이템 데이터가 없거나 플레이어가 범위에 없다면 리턴
        if (itemData == null || !isPlayerInside) return;

        // 2. 플레이어가 충돌 영역 내에 있고 + E키(상호작용)가 눌렸다면 구매 시도
        if (Managers.Input.isInteractPressed)
        {
            TryPurchaseItem();
            Managers.Input.isInteractPressed = false; // 입력 소모 (연속 입력 방지)
        }
    }

    // 2D 환경에서는 뒤에 2D가 붙은 함수를 써야 충돌 감지가 됩니다.
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 충돌한 오브젝트가 플레이어(PlayerBase)인지 컴포넌트로 검사
        if (other.GetComponent<PlayerBase>() != null)
        {
            isPlayerInside = true;

            // 💡 툴팁 UI에 아이템 정보와 위치 전달 (기존 기능 연계)
            if (itemData != null && TooltipUI.Instance != null)
            {
                TooltipUI.Instance.ShowTooltip(itemData, this.transform); //
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<PlayerBase>() != null)
        {
            isPlayerInside = false;

            // 범위를 벗어나면 툴팁을 숨깁니다.
            if (TooltipUI.Instance != null)
            {
                TooltipUI.Instance.HideTooltip(); //
            }
        }
    }

    // 💰 실제 구매를 처리하는 메서드
    private void TryPurchaseItem()
    {
        // 싱글톤 인스턴스 존재 여부 안전하게 체크
        if (Inventory.instance == null)
        {
            Debug.LogError("Inventory 인스턴스가 씬에 없습니다! 구매를 진행할 수 없습니다.");
            return;
        }

        // 💡 [핵심] 인스펙터 입력이 아닌 ItemData 자체에서 가격 정보를 실시간 호출합니다.
        int price = itemData.itemPrice;

        // 보유 골드가 부족한 경우 예외 처리
        if (Inventory.instance.gold < price)
        {
            Debug.LogWarning($"[구매 실패] 골드가 부족합니다. (가격: {price}G / 보유: {Inventory.instance.gold}G)");

            if (HUDLogManager.instance != null)
            {
                HUDLogManager.instance.Log("골드가 부족합니다!", Color.red);
            }
            return;
        }

        // 인벤토리 공간이 넉넉한지 확인 및 가방에 아이템 추가 시도
        bool isAdded = Inventory.instance.Add(itemData); //

        if (isAdded)
        {
            // 아이템 추가에 성공했을 때만 가격만큼 골드를 차감
            Inventory.instance.gold -= price; //
            Debug.Log($"[구매 성공] {itemData.itemName} 구매 완료! 잔액: {Inventory.instance.gold}G");

            // 💡 (선택 사항) 구매 성공 후 상점에서 상품을 지우고 싶다면 아래 코드 활성화
            // TooltipUI.Instance.HideTooltip();
            // Destroy(gameObject);
        }
    }
}