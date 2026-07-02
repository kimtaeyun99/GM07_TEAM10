using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerBase : MonoBehaviour, IDamageable
{
    [SerializeField] protected PlayerData playerData;
    public int maxHp;
    public int currentHp;
    public float moveSpeed;

    public float dodgeCooldowm;
    public WaitForSeconds dodgeCooldownWait;
    public float dodgeDuration;
    public bool isDodgeable { get; set; } = true;
    public bool isDamageable { get; set; } = true;

    [SerializeField] public WeaponBase[] Weapons;
    public WeaponBase currentWeapon;

    public event System.Action<int, int> OnHealthChanged;

    public void Initialize(PlayerData data)
    {
        if (data == null) return;
        playerData = data;

        gameObject.name = playerData.PlayerName;
        maxHp = playerData.MaxHp;
        currentHp = maxHp;
        moveSpeed = playerData.MoveSpeed;
        dodgeCooldowm = playerData.DodgeCooldown;
        dodgeDuration = playerData.DodgeDuration;
        dodgeCooldownWait = new WaitForSeconds(dodgeCooldowm);
    }
    private void Update()
    {
        Attack();
        QuickSlot();
        Reload();
        LookDirection();
    }
    private void Awake()
    {

        for(int i=0; i<Weapons.Length; i++)
        {
            Weapons[i].gameObject.SetActive(false);
        }
    }
    private void Attack()
    {
        if (currentWeapon == null) return;

        if (Managers.Input.isAttackPressed)
        {
            currentWeapon.Shoot();
        }
    }
    private void QuickSlot()
    {
        //if (EquipmentManager.instance == null) return;

        if(Managers.Input.isQuickSlot1Pressed /*&& EquipmentManager.instance.currentEquipment[5] != null*/)
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
        else if(Managers.Input.isQuickSlot2Pressed /*&& EquipmentManager.instance.currentEquipment[6] != null*/)
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
        else if(Managers.Input.isQuickSlot3Pressed /*&& EquipmentManager.instance.currentEquipment[7] != null*/)
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
    }
    private void Reload()
    {
        if (currentWeapon is IReloadable reloadable && Managers.Input.isReloadPressed)
        {
            reloadable.Reload();
        }
        else return;
    }
    public void TakeDamage(int damage)
    {
        if (!isDamageable) return;
        Managers.PlayerAudio.PlayerHit();
        currentHp -= damage;
        Debug.Log($"{gameObject.name} 데미지 받음 ({damage}");
        Debug.Log($"{gameObject.name} 현재 체력 : {currentHp}");

        OnHealthChanged?.Invoke(currentHp,maxHp);
    }
    private bool isFacingLeft = false;
    public void LookDirection()
    {
        if (Managers.Input.movement.x < 0)
        {
            isFacingLeft = true;
        }
        else if (Managers.Input.movement.x > 0)
        {
            isFacingLeft = false;
        }

        if (isFacingLeft)
        {
            transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else
        {
            transform.rotation = Quaternion.identity;
        }
    }
}
