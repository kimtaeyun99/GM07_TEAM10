
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour   // 임시 플레이어 인벤토리 구현
{
    public static PlayerInventory Instance;

    [SerializeField] private Gun[] quickSlot = new Gun[3];   // 플레이어 퀵슬롯
    [SerializeField] private int currentSlotCount = 0;

    public int CurrentSlotCount { get { return currentSlotCount; } }

    public int QuickSlotLength => quickSlot.Length;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public bool SetGun(Gun gun)
    {
        for (int i = 0; i < quickSlot.Length; i++)
        {
            if (quickSlot[i] == null)
            {
                quickSlot[i] = gun;
                return true;
            }
        }
        return false;
    }

    public Gun GetQuickSlot(int index)
    {
        if (index >= quickSlot.Length || index < 0)
        {
            return null;
        }
        return quickSlot[index];
    }

    public void ChangeCurrentSlot(int index)
    {
        currentSlotCount = index;
    }



}
