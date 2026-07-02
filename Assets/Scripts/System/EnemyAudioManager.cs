using UnityEngine;

public class EnemyAudioManager : MonoBehaviour
{
    private void Awake()
    {
        Managers._enemyAudioManager = this;
        DontDestroyOnLoad(gameObject);
    }

    [SerializeField] private AudioSource enemySfxSource;

    [Header("Basic Enemy")]
    [SerializeField] private AudioClip[] basicEnemyWalk;
    [SerializeField] private AudioClip basicEnemyAttack;

    [Header("Elite Enemy")]
    [SerializeField] private AudioClip[] eliteEnemyWalk;
    [SerializeField] private AudioClip eliteEnemyAttack;

    [Header("Boss Enemy")]
    [SerializeField] private AudioClip[] bossEnemyWalk;
    [SerializeField] private AudioClip bossEnemyNonHomingAttack;
    [SerializeField] private AudioClip bossEnemyHomingAttack;

    [Header("Enemy Hit")]
    [SerializeField] private AudioClip enemyHit;

    public void BasicEnemyWalk()
    {
        for(int i=0; i<basicEnemyWalk.Length; i++)
        {
            enemySfxSource.PlayOneShot(basicEnemyWalk[i]);
        }
    }
    public void BasicEnemyAttack()
    {
        enemySfxSource.PlayOneShot(basicEnemyAttack);
    }

    public void EliteEnemyWalk()
    {
        for(int i=0; i<eliteEnemyWalk.Length; i++)
        {
            enemySfxSource.PlayOneShot(eliteEnemyWalk[i]);
        }
    }
    public void EliteEnemyAttack()
    {
        enemySfxSource.PlayOneShot(eliteEnemyAttack);
    }
    public void BossEnemyWalk()
    {
        for(int i=0; i<bossEnemyWalk.Length; i++)
        {
            enemySfxSource.PlayOneShot(bossEnemyWalk[i]);
        }
    }
    public void BossEnemyNonHomingAttack()
    {
        enemySfxSource.PlayOneShot(bossEnemyNonHomingAttack);
    }
    public void BossEnemyHomingAttack()
    {
        enemySfxSource.PlayOneShot(bossEnemyHomingAttack);
    }

    public void EnemyHit()
    {
        enemySfxSource.PlayOneShot(enemyHit);
    }
}
