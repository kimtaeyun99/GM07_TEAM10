using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    private void Awake()
    {
        Managers._playerAudioManager = this;
        DontDestroyOnLoad(gameObject);
    }

    [SerializeField] private AudioSource playerSfxSource;

    [Header("Player")]
    [SerializeField] private AudioClip playerWalk;
    [Range(0f, 1f)] public float playerWalkVolume = 1f;
    [SerializeField] private AudioClip playerDodge;
    [Range(0f, 1f)] public float playerDodgeVolume = 1f;
    [SerializeField] private AudioClip playerHit;
    [Range(0f, 1f)] public float playerHitVolume = 1f;
    [SerializeField] private AudioClip playerDead;
    [Range(0f, 1f)] public float playerDeadVolume = 1f;

    [Header("Pistol")]
    [SerializeField] private AudioClip pistolShoot;
    [Range(0f, 1f)] public float pistolShootVolume = 1f;
    [SerializeField] private AudioClip[] pistolReload;
    [Range(0f, 1f)] public float pistolReloadVolume = 1f;

    [Header("Shotgun")]
    [SerializeField] private AudioClip shotgunShoot;
    [Range(0f, 1f)] public float shotgunShootVolume = 1f;
    [SerializeField] private AudioClip[] shotgunReload;
    [Range(0f, 1f)] public float shotgunReloadVolume = 1f;

    [Header("AR")]
    [SerializeField] private AudioClip arShoot;
    [Range(0f, 1f)] public float arShootVolume = 1f;
    [SerializeField] private AudioClip[] arReload;
    [Range(0f, 1f)] public float arReloadVolume = 1f;
    public void PlayerWalkLoop(bool isWalking)
    {
        if (isWalking)
        {
            if (!playerSfxSource.isPlaying)
            {
                playerSfxSource.clip = playerWalk;
                playerSfxSource.loop = true;
                playerSfxSource.volume = playerWalkVolume;
                playerSfxSource.Play();
            }
        }
        else
        {
            playerSfxSource.Stop();
        }
    }
    public void PlayerDodge()
    {
        playerSfxSource.PlayOneShot(playerDodge, playerDodgeVolume);
    }
    public void PlayerHit()
    {
        playerSfxSource.PlayOneShot(playerHit, playerHitVolume);
    }
    public void PlayerDead()
    {
        playerSfxSource.PlayOneShot(playerDead, playerDeadVolume);
    }
    public void PistolShoot()
    {
        playerSfxSource.PlayOneShot(pistolShoot, pistolShootVolume);
    }
    public void PistolReload()
    {
        for(int i=0; i<pistolReload.Length; i++)
        {
            playerSfxSource.PlayOneShot(pistolReload[i],pistolReloadVolume);
        }
    }
    public void ShotgunShoot()
    {
        playerSfxSource.PlayOneShot(shotgunShoot,shotgunShootVolume);
    }
    public void ShotgunReload()
    {
        for(int i=0; i<shotgunReload.Length; i++)
        {
            playerSfxSource.PlayOneShot(shotgunReload[i],shotgunReloadVolume);
        }
    }   
    public void ARShoot()
    {
        playerSfxSource.PlayOneShot(arShoot,arShootVolume);
    }
    public void ARReload()
    {
        for(int i=0; i<arReload.Length; i++)
        {
            playerSfxSource.PlayOneShot(arReload[i],arReloadVolume);
        }
    }
}
