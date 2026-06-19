using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager instance; // 싱글톤

    // 현재 장착된 아이템들을 담아둘 배열 (0:머리, 1:가슴, 2:무기, 3:신발)
    public ItemData[] currentEquipment = new ItemData[8];

    // 장비창 UI에게 화면을 새로고침하라고 신호를 보낼 이벤트 선언
    public delegate void OnEquipmentChanged();
    public OnEquipmentChanged onEquipmentChangedCallback;

    private void Awake()
    {
        if (instance == null) instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void Equip(ItemData newItem, EquipType targetSlot)
    {
        int slotIndex = (int)targetSlot;

        if (targetSlot == EquipType.Weapon)
        {
            // 1단계: 4번 방(무기1)이 비어있으면 4번으로
            if (currentEquipment[5] == null)
            {
                slotIndex = 5;
            }
            // 2단계: 4번은 차있고, 6번 방(무기2)이 비어있으면 6번으로
            else if (currentEquipment[6] == null)
            {
                slotIndex = 6;
            }
            // 3단계: 4번, 6번 다 차있으면 7번 방(무기3)으로 장착!
            else
            {
                slotIndex = 7;
                Debug.Log("무기 1, 2가 모두 차있어 무기 3(7번 방)에 장착합니다.");

                // (선택) 만약 무기 3번마저 꽉 차 있다면 기존 무기3을 인벤토리로 돌려보냄
                if (currentEquipment[7] != null)
                {
                    if (Inventory.instance != null) Inventory.instance.Add(currentEquipment[7]);
                }
            }
        }

        // 🚨 배열 범위 초과 방지 안전장치 (크기가 7이므로 7 이상일 때 체크)
        if (slotIndex >= currentEquipment.Length) return;

        // 최종 결정된 방 번호(slotIndex)에 아이템 장착
        currentEquipment[slotIndex] = newItem;

        if (Inventory.instance != null)
        {
            Inventory.instance.Remove(newItem);
        }

        if (onEquipmentChangedCallback != null)
        {
            onEquipmentChangedCallback.Invoke();
        }
    }

    // 💡 기존의 해제 함수 (이름은 프로젝트에 따라 Unequip, RemoveEquipment 등 다를 수 있습니다)
    public void Unequip(int slotIndex)
    {
        // 안전장치: 방 번호가 범위를 벗어나거나, 해당 칸이 이미 비어있다면 취소
        if (slotIndex >= currentEquipment.Length || currentEquipment[slotIndex] == null) return;

        // 해제할 아이템을 임시로 기억해 둡니다.
        ItemData unequippedItem = currentEquipment[slotIndex];

        // 💡 핵심: 해당 방을 비워줍니다 (주무기면 4번, 보조무기면 6번 방이 정확히 비워집니다)
        currentEquipment[slotIndex] = null;

        // 해제한 아이템을 플레이어의 인벤토리 가방에 다시 넣어줍니다.
        if (Inventory.instance != null)
        {
            Inventory.instance.Add(unequippedItem);
        }

        // UI를 새로고침하는 콜백 함수 호출
        if (onEquipmentChangedCallback != null)
        {
            onEquipmentChangedCallback.Invoke();
        }
    }
}