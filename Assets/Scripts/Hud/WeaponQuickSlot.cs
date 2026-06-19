using UnityEngine;
using UnityEngine.UI;
using TMPro; // 텍스트 컴포넌트용

public class WeaponQuickSlot : MonoBehaviour
{
    [Header("퀵슬롯 이미지 UI (순서대로 1, 2, 3번)")]
    public Image weapon1Icon;
    public Image weapon2Icon;
    public Image weapon3Icon;

    [Header("빈 슬롯일 때 표시할 기본 스프라이트 (선택)")]
    public Sprite emptySlotSprite;

    void Start()
    {
        // 💡 메인 씬이 로드되면서 EquipmentManager가 생성된 후 이벤트를 연결합니다.
        if (EquipmentManager.instance != null)
        {
            EquipmentManager.instance.onEquipmentChangedCallback += RefreshWeaponQuickSlots;
        }

        // 게임 시작 시 초기 UI 갱신
        RefreshWeaponQuickSlots();
    }

    void OnDestroy()
    {
        // 씬이 전환되거나 파괴될 때 이벤트 구독 해제 (메모리 누수 방지)
        if (EquipmentManager.instance != null)
        {
            EquipmentManager.instance.onEquipmentChangedCallback -= RefreshWeaponQuickSlots;
        }
    }

    // 💡 핵심: 무기 1, 2, 3번의 방 번호(5, 6, 7)에서 데이터를 가져와 UI를 새로고침합니다.
    public void RefreshWeaponQuickSlots()
    {
        if (EquipmentManager.instance == null) return;

        // 장비 매니저의 현재 장착 배열 가져오기
        ItemData[] equipped = EquipmentManager.instance.currentEquipment;

        // 1. 무기 1번 검사 및 업데이트
        UpdateSlotUI(equipped, 5, weapon1Icon);

        // 2. 무기 2번 (6번 방) 검사 및 업데이트
        UpdateSlotUI(equipped, 6, weapon2Icon);

        // 3. 무기 3번 (7번 방) 검사 및 업데이트
        UpdateSlotUI(equipped, 7, weapon3Icon);
    }

    // 슬롯 하나하나의 이미지를 뿌려주는 중복 코드 축약 함수
    private void UpdateSlotUI(ItemData[] equippedArray, int targetIndex, Image slotImage)
    {
        if (slotImage == null) return;

        // 배열 크기 안전장치 및 아이템 존재 여부 확인
        if (targetIndex < equippedArray.Length && equippedArray[targetIndex] != null)
        {
            slotImage.sprite = equippedArray[targetIndex].itemIcon;
            slotImage.color = Color.white; // 불투명하게 켜기
        }
        else
        {
            // 무기가 없으면 투명하게 만들거나 기본 빈 칸 이미지 지정
            if (emptySlotSprite != null)
            {
                slotImage.sprite = emptySlotSprite;
                slotImage.color = Color.white;
            }
            else
            {
                slotImage.sprite = null;
                slotImage.color = Color.clear; // 투명하게 숨기기
            }
        }
    }
}