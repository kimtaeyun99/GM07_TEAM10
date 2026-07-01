using UnityEngine;
using TMPro;

public class TooltipUI : MonoBehaviour
{
    public static TooltipUI Instance;

    [Header("UI Components")]
    public GameObject tooltipWindow;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI typeText;
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI descText;

    [Header("Settings")]
    public Vector3 offset = new Vector3(0, 100f, 0); // 오브젝트 머리 위 여백 (픽셀 단위)

    private Camera mainCamera;
    private Transform targetTransform; // 현재 툴팁을 띄울 대상의 위치

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        mainCamera = Camera.main;
        HideTooltip();
    }

    private void Update()
    {
        // 툴팁이 켜져 있고, 추적할 대상이 있다면 실시간으로 UI 위치를 오브젝트 머리 위로 맞춤
        if (tooltipWindow.activeSelf && targetTransform != null)
        {
            Vector3 screenPos = mainCamera.WorldToScreenPoint(targetTransform.position);
            tooltipWindow.transform.position = screenPos + offset;
        }
    }

    // 설명창 표시 요청 (아이템 데이터와 해당 아이템의 Transform을 받음)
    public void ShowTooltip(ItemData data, Transform itemTransform)
    {
        nameText.text = $"이름 : {data.itemName}";
        typeText.text = $"종류 : {data.itemType}";
        priceText.text = $"가격 : {data.itemPrice}G";
        descText.text = $"설명 : {data.description}";

        targetTransform = itemTransform;
        tooltipWindow.SetActive(true);
    }

    // 설명창 숨기기
    public void HideTooltip()
    {
        targetTransform = null;
        tooltipWindow.SetActive(false);
    }
}