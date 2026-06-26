using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDController : MonoBehaviour
{
    public static HUDController instance;

    [Header("UI Components")]
    public Slider healthSlider;
    public TextMeshProUGUI healthText;

    [Header("Ammo UI")]
    public TextMeshProUGUI ammoText;
    private int currentAmmo = 0;
    private int maxAmmo = 0;

    [Header("Gold UI")]
    public TextMeshProUGUI goldText;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        if (WeaponQuickSlot.instance == null) return;

        // 현재 퀵슬롯 선택 상태 번호 (-1=맨손, 0=권총, 1=샷건, 2=AR)
        int currentWeaponIdx = WeaponQuickSlot.instance.GetActivePrefabIndex();

        // 💡 최신 최적화 함수: 현재 월드(씬)에 다른 사람의 시스템에 의해 생성되어 활성화된 실제 무기 컴포넌트 검색
        WeaponBase activeWeapon = Object.FindAnyObjectByType<WeaponBase>();

        bool isMatched = false;

        if (activeWeapon != null && activeWeapon.gameObject.activeInHierarchy)
        {
            // 🔥 이름 필요 없음! 켜져 있는 컴포넌트의 클래스 기종(C# 타입) 자체를 감시 대조합니다.
            if (currentWeaponIdx == 0 && activeWeapon.gameObject.name.ToLower().Contains("pistol")) isMatched = true;
            else if (currentWeaponIdx == 1 && activeWeapon is Shotgun) isMatched = true; // 샷건 스크립트가 켜져 있을 때
            else if (currentWeaponIdx == 2 && activeWeapon is AR) isMatched = true;           // AR 스크립트가 켜져 있을 때
        }

        // 선택 상태와 켜져 있는 무기가 정상 일치할 때만 실시간 탄약 출력
        if (isMatched && activeWeapon != null)
        {
            SetWeaponAmmo(activeWeapon.currentAmmo, activeWeapon.maxAmmo);
        }
        else
        {
            SetWeaponAmmo(0, 0); // 꺼내지 않았거나 맨손 상태면 - / - 표시
        }
    }

    public void SetWeaponAmmo(int current, int max)
    {
        currentAmmo = current;
        maxAmmo = max;
        UpdateAmmoUI();
    }

    public void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            if (currentAmmo == 0 && maxAmmo == 0) ammoText.text = "- / -";
            else ammoText.text = $"{currentAmmo} / {maxAmmo}";
        }
    }

    public void UpdateHealthUI(float currentHealth)
    {
        if (healthSlider != null) healthSlider.value = currentHealth;
        if (healthText != null) healthText.text = $"{currentHealth} / 100";
    }

    public void UpdateGoldUI(int currentGold)
    {
        if (goldText != null)
        {
            goldText.text = $"{currentGold.ToString("N0")}";
        }
    }
}