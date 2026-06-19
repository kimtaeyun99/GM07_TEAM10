using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBase : MonoBehaviour, IDamageable
{
    protected EnemyData enemyData;
    private Sprite enemySprite;
    protected int currentHp;
    protected int attack;
    protected float moveSpeed;
    protected float damageDelay;
    public WaitForSeconds DamageDelay;
    protected ItemBase goldReward;
    protected ItemBase[] bulletRewards;
    protected float itemDropRadius;

    //public bool IsDamageable { get; private set; } = true;

    protected Sprite sprite;

    public void Initialize(EnemyData data)
    {
        if (data == null) return;
        enemyData = data;

        gameObject.name = enemyData.EnemyName;
        enemySprite = enemyData.EnemySprite;
        currentHp = enemyData.MaxHp;
        attack = enemyData.Attack;
        moveSpeed = enemyData.MoveSpeed;
        damageDelay = enemyData.DamageDelay;
        goldReward = enemyData.GoldReward;
        bulletRewards = enemyData.BulletRewards;
        itemDropRadius = enemyData.ItemDropRadius;
        DamageDelay = new WaitForSeconds(damageDelay);
    }
    private void Awake()
    {
        sprite = GetComponent<Sprite>();
        sprite = enemySprite;
    }
    public void TakeDamage(float damage)
    {
        //if (!IsDamageable) return;
        //IsDamageable = false;
        currentHp -= (int)damage;
        Debug.Log($"{gameObject.name} 데미지 받음 ({damage}");
        Debug.Log($"{gameObject.name} 현재 체력 : {currentHp}");
        if (currentHp <= 0)
        {
            EnemyDie();
        }
        //else
        //{
        //    StartCoroutine(GetDamageDelayCo());
        //}
    }
    protected void EnemyDie()
    {
        Debug.Log($"{gameObject.name} 사망");
        Destroy(gameObject);
        DropRewards();
    }
    protected void DropRewards()
    {
        BulletDrop();
        GoldDrop();
    }
    protected void BulletDrop()
    {
        for (int i = 0; i < bulletRewards.Length; i++)
        {
            //플레이어 무기 리스트를 받아서 해당되는 탄약 드랍하도록 구현
            //플레이어 에서 무기 관리를 어떻게 할지 받은 후 구현 
            int rate = Random.Range(0, 100);
            if (rate <= 30) return;

            Vector2 itemDropOffset = Random.insideUnitCircle * itemDropRadius;
            Vector2 itemSpawnPos = (Vector2)transform.position + itemDropOffset;

            Instantiate(bulletRewards[i], itemSpawnPos, Quaternion.identity);
            Debug.Log($"{bulletRewards[i].name} 드랍");
        }
    }
    protected void GoldDrop()
    {
        Vector2 goldDropOffset = Random.insideUnitCircle * itemDropRadius;
        Vector2 goldSpawnPos = (Vector2)transform.position + goldDropOffset;

        Instantiate(goldReward, goldSpawnPos, Quaternion.identity);
        Debug.Log($"{goldReward.name} 드랍");
    }
    //private IEnumerator GetDamageDelayCo()
    //{
    //    yield return DamageDelay;
    //    IsDamageable = true;
    //}
}
