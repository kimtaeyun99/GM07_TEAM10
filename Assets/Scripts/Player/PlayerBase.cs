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
    private SpriteRenderer weaponSpriteRenderer;

    [SerializeField] private Transform weaponHolder;
    [SerializeField] private Transform firePoint;

    [SerializeField] private WeaponBase[] weaponBases;
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
        weaponSpriteRenderer = transform.Find("Weapon").GetComponent<SpriteRenderer>();

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
        weaponSpriteRenderer = transform.Find("Weapon").GetComponent<SpriteRenderer>();
    }

    private void Move()
    {
        Vector2 movedir = new Vector2(Managers.Input.movement.x, Managers.Input.movement.y);
        rb.linearVelocity = movedir * moveSpeed;
    }
    private void Attack()
    {
        if (!Managers.Input.isAttackPressed) return;
        
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
        yield return dodgeDurationWait;
        isDamageable = true;
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
            EquipWeapon(playerData.PistolSprite);
            weaponHolder.position = new Vector3(0.08f, -0.1f, 0.0f);
            firePoint.position = new Vector3(0.03f, -0.007f, 0.0f);
            Managers.Input.isQuickSlot1Pressed = false;
        }
        if(Managers.Input.isQuickSlot2Pressed)
        {
            EquipWeapon(playerData.ShotgunSprite);
            weaponHolder.position = new Vector3(0f, -0.14f, 0.0f);
            firePoint.position = new Vector3(0.06f, 0.0f, 0.0f);
            Managers.Input.isQuickSlot2Pressed = false;
        }
        if(Managers.Input.isQuickSlot3Pressed)
        {
            EquipWeapon(playerData.ARSprite);
            weaponHolder.position = new Vector3(0f, -0.14f, 0.0f);
            firePoint.position = new Vector3(0.07f, 0f, 0.0f);
            Managers.Input.isQuickSlot3Pressed = false;
        }
        if(Managers.Input.isQuickSlot4Pressed)
        {
            //퀵슬롯4 (Potion) 사용 메서드
            Managers.Input.isQuickSlot4Pressed = false;
        }
    }
    private void EquipWeapon (Sprite weaponSprite)
    {
        weaponSpriteRenderer.sprite = weaponSprite;
        Debug.Log($"{weaponSprite.name} 장착");
    }
    private void Inventory()
    {
        if (!Managers.Input.isInventoryPressed) return;
        //인벤토리 메서드
    }
    private void Reload()
    {
        if (!Managers.Input.isReloadPressed) return;
        //재장전 메서드
    }
    private void SecondaryWeapon()
    {
        if (!Managers.Input.isSecondaryWeaponPressed) return;
        //수류탄 메서드
    }
    public void TakeDamage(float damage)
    {
        if (!isDamageable) return;
        currentHp -= (int)damage;
        Debug.Log($"{gameObject.name} 데미지 받음 ({damage}");
        Debug.Log($"{gameObject.name} 현재 체력 : {currentHp}");
        if (currentHp <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        //플레이어 사망 메서드
    }

}
