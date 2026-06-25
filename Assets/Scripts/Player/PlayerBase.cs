using System.Collections;
using UnityEngine;

public class PlayerBase : MonoBehaviour, IDamageable
{
    protected PlayerData playerData;
    private SpriteRenderer playerSpriteRenderer;
    protected int maxHp;
    protected int currentHp;
    protected float moveSpeed;

    protected float dodgeCooldowm;
    private WaitForSeconds dodgeCooldownWait;
    protected float dodgeDuration;
    private WaitForSeconds dodgeDurationWait;
    public bool isDodgeable { get; private set; } = true;
    public bool isDamageable { get; private set; } = true;

    private Rigidbody2D rb;

    [SerializeField] private WeaponBase[] Weapons;
    private WeaponBase currentWeapon;
    public void Initialize(PlayerData data)
    {
        if (data == null) return;
        playerData = data;

        gameObject.name = playerData.name;
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        if(playerSpriteRenderer != null)
        {
            playerSpriteRenderer.sprite = playerData.PlayerSprite;
        }
        maxHp = playerData.MaxHp;
        currentHp = maxHp;
        moveSpeed = playerData.MoveSpeed;
        dodgeCooldowm = playerData.DodgeCooldown;
        dodgeDuration = playerData.DodgeDuration;

        if(rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        dodgeCooldownWait = new WaitForSeconds(dodgeCooldowm);
        dodgeDurationWait = new WaitForSeconds(dodgeDuration);
    }
    private void FixedUpdate()
    {
        Move();
    }
    private void Update()
    {
        Attack();
        Interact();
        Dodge();
        QuickSlot();
        Inventory();
        Reload();
        SecondaryWeapon();
    }
    private void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        for(int i=0; i<Weapons.Length; i++)
        {
            Weapons[i].gameObject.SetActive(false);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Move()
    {
        Vector2 movedir = new Vector2(Managers.Input.movement.x, Managers.Input.movement.y);
        rb.linearVelocity = movedir * moveSpeed;
    }
    private void Attack()
    {
        if (currentWeapon == null) return;

        // 🚀 1. AR (연사 무기) 처리
        if (currentWeapon is AR arWeapon)
        {
            if (Managers.Input.isAttackPressed)
            {
                // 탄약이 있으면 발사
                if (arWeapon.currentAmmo > 0)
                {
                    arWeapon.Shoot();
                }
                // 💡 [자동 재장전] 탄약이 0이 되는 순간 자동으로 Reload 실행
                else
                {
                    arWeapon.Reload();
                }
            }
        }
        // 💥 2. 샷건 처리 (권총은 항상 탄약이 max이므로 자동으로 통과)
        else if (currentWeapon is Shotgun shotgunWeapon)
        {
            if (UnityEngine.InputSystem.Mouse.current.leftButton.wasPressedThisFrame)
            {
                // 탄약이 있으면 발사
                if (shotgunWeapon.currentAmmo > 0)
                {
                    shotgunWeapon.Shoot();
                }
                // 💡 [자동 재장전] 탄약이 0이 되는 순간 자동으로 Reload 실행
                else
                {
                    shotgunWeapon.Reload();
                }
            }
        }
        // 🔫 3. 권총 등 기타 무기 처리
        else
        {
            if (UnityEngine.InputSystem.Mouse.current.leftButton.wasPressedThisFrame)
            {
                currentWeapon.Shoot();
            }
        }
    }

    private void Interact()
    {
        if (!Managers.Input.isInteractPressed) return;
        //상호작용 메서드
    }
    private void Dodge()
    {
        if (!Managers.Input.isDodgePressed || !isDodgeable) return;
        StartCoroutine(DodgeCo());
        StartCoroutine(DodgeCooldownCo());
    }
    private IEnumerator DodgeCo()
    {
        isDamageable = false;
        Debug.Log("Dodge On");
        yield return dodgeDurationWait;
        isDamageable = true;
        Debug.Log("Dodge Off");
    }
    private IEnumerator DodgeCooldownCo()
    {
        isDodgeable = false;
        yield return dodgeCooldownWait;
        isDodgeable = true;
    }
    private void QuickSlot()
    {
        if(Managers.Input.isQuickSlot1Pressed)
        {
            foreach(WeaponBase weapon in Weapons)
            {
                weapon.StopShoot();
                weapon.gameObject.SetActive(false);
            }
            currentWeapon = Weapons[0];
            currentWeapon.gameObject.SetActive(true);
            Managers.Input.isQuickSlot1Pressed = false;
        }
        if(Managers.Input.isQuickSlot2Pressed)
        {
            foreach (WeaponBase weapon in Weapons)
            {
                weapon.StopShoot();
                weapon.gameObject.SetActive(false);
            }
            currentWeapon = Weapons[1];
            currentWeapon.gameObject.SetActive(true);
            Managers.Input.isQuickSlot2Pressed = false;
        }
        if(Managers.Input.isQuickSlot3Pressed)
        {
            foreach (WeaponBase weapon in Weapons)
            {
                weapon.StopShoot();
                weapon.gameObject.SetActive(false);
            }
            currentWeapon = Weapons[2];
            currentWeapon.gameObject.SetActive(true);
            Managers.Input.isQuickSlot3Pressed = false;
        }
        if(Managers.Input.isQuickSlot4Pressed)
        {
            //퀵슬롯4 (Potion) 사용 메서드
            Managers.Input.isQuickSlot4Pressed = false;
        }
    }
    private void Inventory()
    {
        if (!Managers.Input.isInventoryPressed) return;
        //인벤토리 메서드
    }
    private void Reload()
    {
        if (currentWeapon is IReloadable reloadable && Managers.Input.isReloadPressed)
        {
            reloadable.Reload();
        }
        else return;
    }
    private void SecondaryWeapon()
    {
        if (!Managers.Input.isSecondaryWeaponPressed) return;
        //수류탄 메서드
    }
    public void TakeDamage(int damage)
    {
        if (!isDamageable) return;
        currentHp -= damage;
        Debug.Log($"{gameObject.name} 데미지 받음 ({damage}");
        Debug.Log($"{gameObject.name} 현재 체력 : {currentHp}");
        if (currentHp <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        Destroy(gameObject);
    }

}
