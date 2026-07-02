using UnityEngine;

public class BGMAudioManager : MonoBehaviour
{
    private void Awake()
    {
        Managers._bgmAudioManager = this;
        DontDestroyOnLoad(gameObject);
        PlayBGM();
    }

    [SerializeField] private AudioSource bgmSource;

    [Header("BGM")]
    [SerializeField] private AudioClip bgmClip;
    [Range(0f, 1f)] public float bgmVolume = 1f;

    public void PlayBGM()
    {
        bgmSource.clip = bgmClip;
        bgmSource.loop = true;
        bgmSource.volume = bgmVolume;
        bgmSource.Play();
    }
}
