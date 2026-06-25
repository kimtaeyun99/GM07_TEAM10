using UnityEngine;
using UnityEngine.UI;    // Image, Button 제어를 위해 필수!
using TMPro;             // TextMeshProUGUI 제어를 위해 필수!

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI instance;

    [Header("인벤토리 단축키 설정")]
    public UnityEngine.InputSystem.Key toggleKey = UnityEngine.InputSystem.Key.I;

    [Header("인벤토리 창 세팅")]
    public Transform slotsParent;
    public GameObject inventoryWindow;

    // 💡 [기존 소모품용] 설명 창 UI 세팅
    [Header("소모품 설명 창 UI 세팅")]
    public GameObject descriptionPanel;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescText;
    public Button useButton;
    public Button QuickButton;
    public Image itemDetailIcon;

    // 💡 [추가: 장비 전용] 상세 패널 UI 세팅
    [Header("장비 상세 패널 UI 세팅")]
    public GameObject equipDetailPanel;    // 🛠️ 새로 만드실 장비 전용 패널 오브젝트
    public TextMeshProUGUI equipNameText;  // 장비 이름 텍스트
    public TextMeshProUGUI equipDescText;  // 장비 설명 텍스트 (능력치 등)
    public Image equipDetailIcon;          // 장비 이미지
    public Button equipButton;             // 장비창의 '장착' 버튼 (기존 사용 버튼 대신 연결 가능)

    private Inventory inventory;
    private InventorySlot[] slots;

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
            Debug.LogError("InventoryUI: 씬에서 Inventory 시스템을 찾을 수 없습니다.");
            return;
        }

        if (slotsParent != null)
        {
            slots = slotsParent.GetComponentsInChildren<InventorySlot>();
        }

        // 버튼 리스너 등록
        if (useButton != null) useButton.onClick.AddListener(OnUseButtonClick);
        if (QuickButton != null) QuickButton.onClick.AddListener(OnQuickRegisterButtonClick);

        // 💡 장비 장착 버튼도 기존의 장착 로직(OnUseButtonClick)을 공유하거나 따로 등록 가능합니다.
        if (equipButton != null) equipButton.onClick.AddListener(OnUseButtonClick);

        if (inventoryWindow != null) inventoryWindow.SetActive(false);

        // 두 패널 모두 시작할 때 비활성화
        if (descriptionPanel != null) descriptionPanel.SetActive(false);
        if (equipDetailPanel != null) equipDetailPanel.SetActive(false);
    }

    void Update()
    {
        if (UnityEngine.InputSystem.Keyboard.current != null)
        {
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
                // 💡 닫힐 때 모든 패널을 안전하게 꺼줍니다.
                CloseAllPanels();
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

    // 💡 핵심 변경 항목: 타입에 따라 서로 다른 패널을 활성화하는 함수
    public void ShowDescription(InventoryItem slotItem, int index)
    {
        selectedSlotItem = slotItem;
        selectedSlotIndex = index;
        ItemData item = slotItem.itemData;

        // 먼저 열려있던 패널들을 정리하고 시작합니다.
        CloseAllPanels();

        // 💡 1. 소모품(Consumable)일 때 -> 기존 디스크립션 패널 오픈
        if (item.itemType == ItemType.Consumable)
        {
            if (itemNameText != null) itemNameText.text = item.itemName;
            if (itemDescText != null) itemDescText.text = item.description;
            if (itemDetailIcon != null) itemDetailIcon.sprite = item.itemIcon;

            if (descriptionPanel != null) descriptionPanel.SetActive(true);
        }
        // 💡 2. 장착품(Equipment)일 때 -> 별도의 장비 상세 패널 오픈
        else if (item.itemType == ItemType.Equipment)
        {
            if (equipNameText != null) equipNameText.text = item.itemName;
            if (equipDescText != null) equipDescText.text = item.description; // 나중에 필요시 무기 공격력 등의 정보 확장 가능
            if (equipDetailIcon != null) equipDetailIcon.sprite = item.itemIcon;

            if (equipDetailPanel != null) equipDetailPanel.SetActive(true);
        }
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

            // 사용 완료 후 패널 닫기
            CloseAllPanels();
            selectedSlotItem = null;
        }
    }

    // 💡 UI 상태를 초기화하기 위한 헬퍼 함수
    private void CloseAllPanels()
    {
        if (descriptionPanel != null) descriptionPanel.SetActive(false);
        if (equipDetailPanel != null) equipDetailPanel.SetActive(false);
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