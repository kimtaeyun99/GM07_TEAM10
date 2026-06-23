using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDController : MonoBehaviour
{
    // 💡 어디서나 골드를 추가하고 UI를 갱신할 수 있도록 싱글톤 인스턴스 추가
    public static HUDController instance;

    [Header("UI Components")]
    public Slider healthSlider;
    public TextMeshProUGUI healthText;

    private float maxHealth = 100f;
    private float currentHealth = 100f;

    [Header("Ammo UI")]
    public TextMeshProUGUI ammoText;    // "30 / 270" 텍스트

    private int currentAmmo = 30;
    private int maxAmmo = 30;

    [Header("Gold UI")]
    public TextMeshProUGUI goldText;    // 골드 숫자를 표시할 TextMeshProUGUI 연결
    private int currentGold = 0;

    void Awake()
    {
        // 싱글톤 초기화
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateHealthUI(currentHealth);
        UpdateAmmoUI();
        // 💡 게임 시작 시 골드 UI 초기화
        UpdateGoldUI();
    }

    public void UpdateHealthUI(float currentHealth)
    {
        // 최댓값 설정 (Start에서 한 번만 해줘도 됩니다)
        healthSlider.maxValue = maxHealth;

        // 슬라이더의 Value만 바꾸면 유니티가 알아서 게이지를 조절합니다.
        healthSlider.value = currentHealth;

        // 텍스트도 함께 업데이트
        healthText.text = $"{currentHealth} / {maxHealth}";
    }

    public void UpdateAmmoUI()
    {
        // 탄약 텍스트 업데이트
        ammoText.text = $"{currentAmmo} / {maxAmmo}";
    }

    // 체력이 닳거나 회복될 때 이 함수들을 다른 게임매니저에서 호출하게 합니다.
    public void SetHealth(float health)
    {
        currentHealth = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI(currentHealth);
    }

    public void AddGold(int amount)
    {
        currentGold += amount;

        // 골드가 0원 미만으로 떨어지지 않도록 방지
        if (currentGold < 0)
        {
            currentGold = 0;
        }

        // 수량이 변경되었으므로 UI 새로고침
        UpdateGoldUI();
    }

    public void UpdateGoldUI()
    {
        if (goldText != null)
        {
            // N0을 붙여주면 천 단위마다 쉼표(,)가 자동으로 찍힙니다 (예: 50,000)
            goldText.text = currentGold.ToString("N0");

        }
    }

    public int GetCurrentGold()
    {
        return currentGold;
    }
}