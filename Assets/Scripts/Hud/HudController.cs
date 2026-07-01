using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; // 씬 관리를 위해 필수

public class HUDController : MonoBehaviour
{
    public static HUDController instance;

    [Header("UI Components")]
    public Slider healthSlider;
    public TextMeshProUGUI healthText;

    [Header("Tutorial Settings")]
    [SerializeField] private GameObject guidePanel;          // 유니티 에디터에서 꺼둔(Inactive) 가이드 패널 등록
    [SerializeField] private string tutorialSceneName = "Stage_Tutorial"; // 튜토리얼 씬의 정확한 이름

    [Header("Ammo UI")]
    public TextMeshProUGUI ammoText;
    private int currentAmmo = 0;
    private int maxAmmo = 0;

    [Header("Gold UI")]
    public TextMeshProUGUI goldText;

    private void Start()
    {
        PlayerBase player = FindAnyObjectByType<PlayerBase>();
        if (player != null)
        {
            player.OnHealthChanged += UpdateHealthUI;
            UpdateHealthUI(player.currentHp, player.maxHp);
        }

        // 💡 [중요] 게임이 '처음' 실행된 씬이 만약 튜토리얼 씬일 경우를 위해 Start에서도 한 번 체크해줍니다.
        CheckAndToggleGuidePanel(SceneManager.GetActiveScene().name);
    }

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        Canvas hudCanvas = GetComponentInChildren<Canvas>(true);

        if (hudCanvas != null)
        {
            hudCanvas.enabled = true;
            hudCanvas.gameObject.SetActive(true);
        }

        // 씬 로드 이벤트 리스너 등록
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // 이벤트 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 씬이 전환되어 로드가 완료되면 유니티가 자동으로 호출해주는 함수
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 💡 씬이 시작되면 이 함수를 통해 패널을 켜고 끕니다.
        CheckAndToggleGuidePanel(scene.name);
    }

    // 💡 가이드 패널의 활성화 여부를 판단하는 핵심 로직 분리
    private void CheckAndToggleGuidePanel(string sceneName)
    {
        if (guidePanel == null) return;

        // 가이드 패널이 꺼져있더라도, 튜토리얼 씬이 시작되면 확실하게 True(활성화)로 만듭니다.
        if (sceneName == tutorialSceneName)
        {
            guidePanel.SetActive(true);
            Debug.Log($"[HUDController] {sceneName} 시작됨 -> 가이드 패널 활성화!");
        }
        else
        {
            guidePanel.SetActive(false);
            Debug.Log($"[HUDController] 일반 씬({sceneName}) 시작됨 -> 가이드 패널 비활성화.");
        }
    }

    private void FixedUpdate()
    {
        if (WeaponQuickSlot.instance == null) return;

        int currentWeaponIdx = WeaponQuickSlot.instance.GetActivePrefabIndex();
        WeaponBase activeWeapon = Object.FindAnyObjectByType<WeaponBase>();

        bool isMatched = false;

        if (activeWeapon != null && activeWeapon.gameObject.activeInHierarchy)
        {
            if (currentWeaponIdx == 0 && activeWeapon.gameObject.name.ToLower().Contains("pistol")) isMatched = true;
            else if (currentWeaponIdx == 1 && activeWeapon is Shotgun) isMatched = true;
            else if (currentWeaponIdx == 2 && activeWeapon is AR) isMatched = true;
        }

        if (isMatched && activeWeapon != null)
        {
            SetWeaponAmmo(activeWeapon.currentAmmo, activeWeapon.maxAmmo);
        }
        else
        {
            SetWeaponAmmo(0, 0);
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

    public void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
        if (healthText != null) healthText.text = $"{currentHealth} / {maxHealth}";
    }
    public void UpdateGoldUI(int currentGold)
    {
        if (goldText != null)
        {
            goldText.text = $"{currentGold.ToString("N0")}";
        }
    }
}