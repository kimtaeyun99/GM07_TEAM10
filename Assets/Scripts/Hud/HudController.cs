using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDController : MonoBehaviour
{
    [Header("UI Components")]
    public Slider healthSlider;        // 1단계에서 만든 Slider 오브젝트를 연결
    public TextMeshProUGUI healthText; // "124 / 150" 텍스트 연결

    private float maxHealth = 100f;
    private float currentHealth = 100f;

    [Header("Ammo UI")]
    public TextMeshProUGUI ammoText;    // "30 / 270" 텍스트

    private int currentAmmo = 30;
    private int maxAmmo = 30;

    void Start()
    {
        UpdateHealthUI(currentHealth);
        UpdateAmmoUI();
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
}