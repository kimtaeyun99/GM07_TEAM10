using UnityEngine;
using UnityEngine.UI;    // Image, Button 제어를 위해 필수!
using TMPro;             // TextMeshProUGUI 제어를 위해 필수!

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI instance;

    [Header("인벤토리 창 세팅")]
    public Transform slotsParent; // SlotHolder를 연결할 곳
    public GameObject inventoryWindow; // I키로 꺼고 켤 인벤토리 전체 창

    [Header("우측 설명 창 UI 세팅")]
    public GameObject descriptionPanel;   // 우측 설명 패널 (DescriptionPanel)
    public TextMeshProUGUI itemNameText;  // 아이템 이름 텍스트
    public TextMeshProUGUI itemDescText;  // 아이템 설명 텍스트
    public Button useButton;              // 사용 버튼 (UseButton)
    public Image itemDetailIcon;          // 우측 창에 표시될 아이템 이미지

    private Inventory inventory;
    private InventorySlot[] slots; // 자식 슬롯들의 배열

    // 💡 변경: 이제 단순 ItemData가 아니라 수량 정보가 묶인 인벤토리 칸 데이터를 기억합니다.
    private InventoryItem selectedSlotItem;
    private int selectedSlotIndex;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (inventory == null)
        {
            inventory = Inventory.instance;

            if (inventory == null)
            {
#pragma warning disable CS0618
                inventory = FindObjectOfType<Inventory>();
#pragma warning restore CS0618
            }
        }

        if (inventory != null)
        {
            inventory.onItemChangedCallback += UpdateUI;
        }
        else
        {
            Debug.LogError("InventoryUI: 씬에서 Inventory 시스템(매니저)을 찾을 수 없습니다! 하이어라키에 Inventory 스크립트가 붙은 오브젝트가 있는지 확인하세요.");
            return;
        }

        if (slotsParent != null)
        {
            slots = slotsParent.GetComponentsInChildren<InventorySlot>();
        }

        if (useButton != null)
        {
            useButton.onClick.AddListener(OnUseButtonClick);
        }

        if (inventoryWindow != null) inventoryWindow.SetActive(false);
        if (descriptionPanel != null) descriptionPanel.SetActive(false);
    }

    void Update()
    {
        if (UnityEngine.InputSystem.Keyboard.current != null && UnityEngine.InputSystem.Keyboard.current.iKey.wasPressedThisFrame)
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        if (inventoryWindow != null)
        {
            bool isActive = !inventoryWindow.activeSelf;
            inventoryWindow.SetActive(isActive);

            if (isActive)
            {
                UpdateUI();
            }
            else
            {
                if (descriptionPanel != null) descriptionPanel.SetActive(false);
                selectedSlotItem = null; // 💡 변수명 변경 반영
            }
        }
    }

    // 💡 변경: 슬롯을 클릭했을 때 단순 데이터가 아닌 개수가 포함된 InventoryItem을 받도록 수정
    public void ShowDescription(InventoryItem slotItem, int index)
    {
        selectedSlotItem = slotItem;
        selectedSlotIndex = index; // 💡 방 번호 저장!

        ItemData item = slotItem.itemData;
        if (itemNameText != null) itemNameText.text = item.itemName;
        if (itemDescText != null) itemDescText.text = item.description;
        if (itemDetailIcon != null) itemDetailIcon.sprite = item.itemIcon;

        if (descriptionPanel != null) descriptionPanel.SetActive(true);
    }

    // [사용] 버튼을 눌렀을 때 실행되는 함수 수정
    public void OnUseButtonClick()
    {
        if (selectedSlotItem != null && selectedSlotItem.itemData != null)
        {
            ItemData item = selectedSlotItem.itemData;

            if (item.itemType == ItemType.Equipment)
            {
                if (EquipmentManager.instance != null)
                {
                    EquipmentManager.instance.Equip(item, item.targetEquipSlot);
                    // 💡 장비도 장착하면 가방에서 사라져야 하므로, 이름을 찾아서 지우는게 아니라 정확한 그 방 번호를 지웁니다.
                    inventory.RemoveAt(selectedSlotIndex);
                }
            }
            else if (item.itemType == ItemType.Consumable)
            {
                item.Use();
                // 💡 핵심: 기존의 inventory.Remove(item) 대신 정확히 내가 클릭했던 방 번호를 지우라고 명령합니다!
                inventory.RemoveAt(selectedSlotIndex);
            }

            if (descriptionPanel != null) descriptionPanel.SetActive(false);
            selectedSlotItem = null;
        }
    }

    // InventoryUI.cs 의 UpdateUI() 함수 내부
    public void UpdateUI()
    {
        if (slots == null || inventory == null) return;

        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                // 💡 변경: slots[i].AddItem에 현재 루프 번호인 'i'를 같이 넘겨줍니다!
                slots[i].AddItem(inventory.items[i], i);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
}