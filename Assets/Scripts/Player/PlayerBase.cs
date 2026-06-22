using UnityEngine;

public class PlayerBase : MonoBehaviour, IDamageable
{
    protected PlayerData playerData;
    private SpriteRenderer playerSpriteRenderer;
    protected int maxHp;
    protected int currentHp;
    protected float moveSpeed;
    protected float dodgeCooldowm;
    protected float dodgeDuration;

    private Rigidbody2D rb;

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
    
    private void Move()
    {
        Vector2 movedir = new Vector2(InputManager.Instance.movement.x, InputManager.Instance.movement.y);
        rb.linearVelocity = movedir * moveSpeed;
    }
    private void Attack()
    {
        if (!InputManager.Instance.isAttackPressed) return;
        //공격 메서드
    }
    private void Interact()
    {
        if (!InputManager.Instance.isInteractPressed) return;
        //상호작용 메서드
    }
    private void Dodge()
    {
        if (!InputManager.Instance.isDodgePressed) return;
        //회피 메서드
    }
    private void QuickSlot()
    {
        if(InputManager.Instance.isQuickSlot1Pressed)
        {
            //퀵슬롯1 (Pistol) 전환 메서드
            InputManager.Instance.isQuickSlot1Pressed = false;
        }
        if(InputManager.Instance.isQuickSlot2Pressed)
        {
            //퀵슬롯2 (Shotgun) 전환 메서드
            InputManager.Instance.isQuickSlot2Pressed = false;
        }
        if(InputManager.Instance.isQuickSlot3Pressed)
        {
            //퀵슬롯3 (AR) 전환 메서드
            InputManager.Instance.isQuickSlot3Pressed = false;
        }
        if(InputManager.Instance.isQuickSlot4Pressed)
        {
            //퀵슬롯4 (Potion) 사용 메서드
            InputManager.Instance.isQuickSlot4Pressed = false;
        }
    }
    private void Inventory()
    {
        if (!InputManager.Instance.isInventoryPressed) return;
        //인벤토리 메서드
    }
    private void Reload()
    {
        if (!InputManager.Instance.isReloadPressed) return;
        //재장전 메서드
    }
    private void SecondaryWeapon()
    {
        if (!InputManager.Instance.isSecondaryWeaponPressed) return;
        //수류탄 메서드
    }
    public void TakeDamage(float damage)
    {
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
