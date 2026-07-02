using System.Collections;
using System.Runtime.CompilerServices;
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
    [SerializeField] private AudioClip basicEnemyAttack;
    [Range(0f, 1f)] public float basicEnemyAttackVolume = 1f;
    [SerializeField] private AudioClip basicEnemyDie;
    [Range(0f, 1f)] public float basicEnemyDieVolume = 1f;

    [Header("Elite Enemy")]
    [SerializeField] private AudioClip[] eliteEnemyWalk;
    [Range(0f, 1f)] public float eliteEnemyWalkVolume = 1f;
    [SerializeField] private AudioClip eliteEnemyAttack;
    [Range(0f, 1f)] public float eliteEnemyAttackVolume = 1f;
    [SerializeField] private AudioClip eliteEnemyDie;
    [Range(0f, 1f)] public float eliteEnemyDieVolume = 1f;
    private int eWalk = 0;

    [Header("Boss Enemy")]
    [SerializeField] private AudioClip[] bossEnemyWalk;
    [Range(0f, 1f)] public float bossEnemyWalkVolume = 1f;
    [SerializeField] private AudioClip bossEnemyNonHomingAttack;
    [Range(0f, 1f)] public float bossEnemyNonHomingAttackVolume = 1f;
    [SerializeField] private AudioClip bossEnemyHomingAttack;
    [Range(0f, 1f)] public float bossEnemyHomingAttackVolume = 1f;
    [SerializeField] private AudioClip[] bossEnemyDie;
    [Range(0f, 1f)] public float bossEnemyDieVolume = 1f;
    private int bWalk = 0;

    [Header("Enemy Hit")]
    [SerializeField] private AudioClip enemyHit;
    [Range(0f, 1f)] public float enemyHitVolume = 1f;

    [SerializeField] private float walkInterval = 0.4f;

    private float eliteEnemyWalkTimer = 0f;
    private float bossEnemyWalkTimer = 0f;

    public void BasicEnemyAttack()
    {
        enemySfxSource.PlayOneShot(basicEnemyAttack,basicEnemyAttackVolume);
    }
    public void BasicEnemyDie()
    {
        enemySfxSource.PlayOneShot(basicEnemyDie, basicEnemyDieVolume);
    }
    public void EliteEnemyWalk()
    {
        eliteEnemyWalkTimer += Time.deltaTime;
        if (eliteEnemyWalkTimer >= walkInterval)
        {
            enemySfxSource.PlayOneShot(eliteEnemyWalk[eWalk], eliteEnemyWalkVolume);
            eWalk++;
            if (eWalk >= eliteEnemyWalk.Length) eWalk = 0;
            eliteEnemyWalkTimer = 0f;
        }
    }
    public void EliteEnemyAttack()
    {
        enemySfxSource.PlayOneShot(eliteEnemyAttack,eliteEnemyAttackVolume);
    }
    public void EliteEnemyDie()
    {
        enemySfxSource.PlayOneShot(eliteEnemyDie, eliteEnemyDieVolume);
    }
    public void BossEnemyWalk()
    {
        bossEnemyWalkTimer += Time.deltaTime;
        if (bossEnemyWalkTimer >= walkInterval)
        {
            enemySfxSource.PlayOneShot(bossEnemyWalk[bWalk], bossEnemyWalkVolume);
            bWalk++;
            if (bWalk >= bossEnemyWalk.Length) bWalk = 0;
            bossEnemyWalkTimer = 0f;
        }
    }
    public void BossEnemyNonHomingAttack()
    {
        enemySfxSource.PlayOneShot(bossEnemyNonHomingAttack,bossEnemyNonHomingAttackVolume);
    }
    public void BossEnemyHomingAttack()
    {
        enemySfxSource.PlayOneShot(bossEnemyHomingAttack,bossEnemyHomingAttackVolume);
    }
    public void BossEnemyDie()
    {
        StartCoroutine(BossEnemyDieCo());
    }
    private IEnumerator BossEnemyDieCo()
    {
        for (int i = 0; i < bossEnemyDie.Length; i++)
        {
            enemySfxSource.PlayOneShot(bossEnemyDie[i], bossEnemyDieVolume);
            yield return new WaitForSeconds(bossEnemyDie[i].length); // 클립 길이만큼 기다림
        }
    }
    public void EnemyHit()
    {
        enemySfxSource.PlayOneShot(enemyHit,enemyHitVolume);
    }
}
