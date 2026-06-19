using UnityEngine;

public class PlayerStats : MonoBehaviour, IDamageable
{
    [SerializeField] private CharacterData data;
    private int gold = 0;

    public static PlayerStats Instacne;
    public float MoveSpeed { get; private set; } // 이동속도
    public float MaxHealth { get; private set; } // 최대체력
    public float CurrentHealth { get; private set; } // 현재 체력

    public float DodgeSpeed { get; private set; } // 구르기 , 회피 속도

    public float DodgeCooltime { get; private set; }

    public Transform Transform { get; private set; }

    public float DodgeDuration { get; private set; }
    public bool IsDodge { get; set; }
    public int Gold { get; private set; }


    private void Awake()
    {
        InitializeStats(); // 초기화

        if (Instacne == null)  // 싱글톤
        {
            Instacne = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void InitializeStats() // 플레이어 데이터 초기화
    {
        MoveSpeed = data.moveSpeed;
        MaxHealth = data.maxHealth;
        CurrentHealth = MaxHealth;
        DodgeSpeed = data.dodgeSpeed;
        DodgeCooltime = data.dodgeCooltime;
        DodgeDuration = data.dodgeDuration;
        Transform = transform;
    }

    public void TakeDamage(float damage) // IDamageable 
    {
        CurrentHealth -= damage;
        if (CurrentHealth <= 0f)
        {
            CurrentHealth = 0f;
            Die();
        }
    }

    public void Heal(float amount)
    {
        CurrentHealth = Mathf.Min(CurrentHealth + amount, MaxHealth); // 최대체력은 넘지않게 
    }

    private void Die()
    {
        Debug.Log($"{data.name} 사망");
    }
}
