using UnityEngine;

public class ItemTest : MonoBehaviour
{
    public ItemData testItem; // 테스트할 아이템 데이터
    [Header("테스트용 장비 아이템 데이터")]
    public ItemData testWeapon;
    public ItemData testArmor1;
    public ItemData testArmor2;
    public ItemData testHelmet;
    public ItemData testBoots;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (testItem != null)
                Inventory.instance.Add(testItem);
        }

        if (UnityEngine.InputSystem.Keyboard.current != null && UnityEngine.InputSystem.Keyboard.current.digit6Key.wasPressedThisFrame)
        {
            if (testWeapon != null)
            {
                Inventory.instance.Add(testWeapon);
                Debug.Log("인벤토리에 테스트 무기를 추가했습니다!");
            }
        }

        // 💡 숫자 키를 누르면 투구 아이템이 가방에 들어옵니다.
        if (UnityEngine.InputSystem.Keyboard.current != null && UnityEngine.InputSystem.Keyboard.current.digit7Key.wasPressedThisFrame)
        {
            if (testArmor1 != null)
            {
                Inventory.instance.Add(testArmor1);
                Debug.Log("인벤토리에 테스트 방어구를 추가했습니다!");
            }
        }

        if (UnityEngine.InputSystem.Keyboard.current != null && UnityEngine.InputSystem.Keyboard.current.digit8Key.wasPressedThisFrame)
        {
            if (testArmor2 != null)
            {
                Inventory.instance.Add(testArmor2);
                Debug.Log("인벤토리에 테스트 방어구를 추가했습니다!");
            }
        }

        if (UnityEngine.InputSystem.Keyboard.current != null && UnityEngine.InputSystem.Keyboard.current.digit9Key.wasPressedThisFrame)
        {
            if (testHelmet != null)
            {
                Inventory.instance.Add(testHelmet);
                Debug.Log("인벤토리에 테스트 방어구를 추가했습니다!");
            }
        }

        if (UnityEngine.InputSystem.Keyboard.current != null && UnityEngine.InputSystem.Keyboard.current.digit0Key.wasPressedThisFrame)
        {
            if (testBoots != null)
            {
                Inventory.instance.Add(testBoots);
                Debug.Log("인벤토리에 테스트 방어구를 추가했습니다!");
            }
        }
    }
}