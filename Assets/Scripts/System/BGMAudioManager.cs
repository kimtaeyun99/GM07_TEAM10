using UnityEngine;

public class BGMAudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource bgmSource;

    [Header("BGM")]
    [SerializeField] private AudioClip bgmClip;
    [Range(0f, 1f)] public float bgmVolume = 1f;

    public void PlayBGM()
    {
        bgmSource.clip = bgmClip;
        bgmSource.loop = true;
        bgmSource.Play();
    }
}
