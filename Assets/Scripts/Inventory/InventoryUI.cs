using UnityEngine;
using UnityEngine.UI;    // Image, Button 제어를 위해 필수!
using TMPro;             // TextMeshProUGUI 제어를 위해 필수!

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI instance;

    // --- InventoryUI.cs 인스펙터 설정 변수 추가 ---
    [Header("인벤토리 단축키 설정")]
    public UnityEngine.InputSystem.Key toggleKey = UnityEngine.InputSystem.Key.I;

    [Header("인벤토리 창 세팅")]
    public Transform slotsParent; // SlotHolder를 연결할 곳
    public GameObject inventoryWindow; // I키로 켜고 켤 인벤토리 전체 창

    [Header("우측 설명 창 UI 세팅")]
    public GameObject descriptionPanel;   // 우측 설명 패널 (DescriptionPanel)
    public TextMeshProUGUI itemNameText;  // 아이템 이름 텍스트
    public TextMeshProUGUI itemDescText;  // 아이템 설명 텍스트
    public Button useButton;              // 사용 버튼 (UseButton)
    public Button QuickButton;
    public Image itemDetailIcon;          // 우측 창에 표시될 아이템 이미지

    private Inventory inventory;
    private InventorySlot[] slots; // 자식 슬롯들의 배열

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
            inventory.onItemChangedCallback += RefreshConsumableHUD;
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
        if (QuickButton != null)
        {
            QuickButton.onClick.AddListener(OnQuickRegisterButtonClick);
        }

        // 💡 중요: 스크립트가 붙은 최상위 오브젝트(InventoryUI)는 절대 끄지 않고,
        // 실제 화면에 보이는 자식 창(inventoryWindow)만 비활성화합니다.
        if (inventoryWindow != null) inventoryWindow.SetActive(false);
        if (descriptionPanel != null) descriptionPanel.SetActive(false);
    }

    void Update()
    {
        if (UnityEngine.InputSystem.Keyboard.current != null)
        {
            // 💡 드롭다운에서 선택한 Key 값을 다이렉트로 대입하여 감지합니다.
            if (UnityEngine.InputSystem.Keyboard.current[toggleKey].wasPressedThisFrame)
            {
                ToggleInventory();
            }
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
                selectedSlotItem = null;
            }
        }
    }

    private void RefreshConsumableHUD()
    {
        if (ConsumableQuickSlot.instance != null)
        {
            ConsumableQuickSlot.instance.RefreshSlots();
        }
    }

    public void OnQuickRegisterButtonClick()
    {
        if (selectedSlotItem != null && selectedSlotItem.itemData != null)
        {
            if (selectedSlotItem.itemData.itemType == ItemType.Consumable)
            {
                if (ConsumableQuickSlot.instance != null)
                {
                    ConsumableQuickSlot.instance.RegisterToNextAvailableSlot(selectedSlotItem);
                }
            }
        }
    }

    public void ShowDescription(InventoryItem slotItem, int index)
    {
        selectedSlotItem = slotItem;
        selectedSlotIndex = index;

        ItemData item = slotItem.itemData;
        if (itemNameText != null) itemNameText.text = item.itemName;
        if (itemDescText != null) itemDescText.text = item.description;
        if (itemDetailIcon != null) itemDetailIcon.sprite = item.itemIcon;

        if (descriptionPanel != null) descriptionPanel.SetActive(true);
    }

    public void OnUseButtonClick()
    {
        if (selectedSlotItem != null && selectedSlotItem.itemData != null)
        {
            ItemData item = selectedSlotItem.itemData;

            if (item.itemType == ItemType.Equipment)
            {
                if (EquipmentManager.instance != null)
                {
                    EquipmentManager.instance.Equip(item, item.targetEquipSlot, selectedSlotIndex);
                }
            }
            else if (item.itemType == ItemType.Consumable)
            {
                item.Use();
                inventory.RemoveAt(selectedSlotIndex);
            }

            if (descriptionPanel != null) descriptionPanel.SetActive(false);
            selectedSlotItem = null;
        }
    }

    public void UpdateUI()
    {
        if (slots == null || inventory == null) return;

        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i], i);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
}