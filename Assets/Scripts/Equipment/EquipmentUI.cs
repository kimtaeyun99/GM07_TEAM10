using UnityEngine;

public class EquipmentUI : MonoBehaviour
{
    public EquipmentSlot[] uiSlots;

    void Start()
    {
        if (EquipmentManager.instance == null)
        {
#pragma warning disable CS0618
            EquipmentManager.instance = FindObjectOfType<EquipmentManager>();
#pragma warning restore CS0618
        }

        if (EquipmentManager.instance != null)
        {
            EquipmentManager.instance.onEquipmentChangedCallback += UpdateEquipmentUI;
        }
        else
        {
            Debug.LogError("🚨 [EquipmentUI 에러] 씬에 EquipmentManager 오브젝트가 아예 없습니다! 빈 오브젝트를 만들고 스크립트를 붙이세요.");
            return;
        }

        UpdateEquipmentUI();
    }

    void UpdateEquipmentUI()
    {
        // 💡 추적 1: 매니저 자체가 없는지 검사
        if (EquipmentManager.instance == null) return;

        // 💡 추적 2: 매니저 내부의 장착 배열(리스트) 공간이 아직 안 만들어졌는지 검사
        if (EquipmentManager.instance.currentEquipment == null)
        {
            Debug.LogError("🚨 [EquipmentUI 에러] EquipmentManager 내부의 currentEquipment 배열이 생성되지 않았습니다! Manager 스크립트에서 선언과 동시에 = new ItemData[4]; 를 해주세요.");
            return;
        }

        // 💡 추적 3: UI 슬롯 배열 자체가 비어있는지 검사
        if (uiSlots == null || uiSlots.Length == 0)
        {
            Debug.LogError("🚨 [EquipmentUI 에러] 인스펙터 창의 Ui Slots 배열 Size가 0이거나 비어있습니다!");
            return;
        }

        for (int i = 1; i < uiSlots.Length; i++)
        {
            // 💡 추적 4: 슬롯들 중 하나라도 연결이 빠졌는지 검사
            if (uiSlots[i] == null)
            {
                Debug.LogError($"🚨 [EquipmentUI 에러] 인스펙터 창의 Ui Slots 배열 중 {i}번째 칸(Element {i})이 비어있습니다(None)! 하이어라키에서 슬롯을 드래그해서 채우세요.");
                continue;
            }

            // 안전함이 검증되었으니 실행
            ItemData item = EquipmentManager.instance.currentEquipment[i];

            if (item != null)
            {
                // 💡 이 로그를 추가해 보세요!
                Debug.Log($"[UI 출력 체크] {i}번 슬롯({uiSlots[i].name})에 아이템 {item.itemName}을 그리려고 시도합니다.");

                uiSlots[i].DisplayItem(item);
            }
            else
            {
                uiSlots[i].ClearSlot();
            }
        }
    }
}