using UnityEngine;

public static class Managers
{
    private static InputManager _inputManager;

    public static InputManager Input
    {
        get
        {
            if(_inputManager == null)
            {
                GameObject obj = new GameObject("InputManager");
                _inputManager = obj.AddComponent<InputManager>();
                Object.DontDestroyOnLoad(obj);
            }
            return _inputManager;
        }
    }

    private static PoolManager _poolManager;

    public static PoolManager Pool
    {
        get
        {
            if(_poolManager == null)
            {
                GameObject obj = new GameObject("PoolManager");
                _poolManager = obj.AddComponent<PoolManager>();
                Object.DontDestroyOnLoad(obj);
            }
            return _poolManager;
        }
    }
    private static CameraManager _cameraManager;

    public static CameraManager Camera
    {
        get
        {
            if (_cameraManager == null)
            {
                GameObject obj = new GameObject("MainCamera");
                _cameraManager = obj.AddComponent<CameraManager>();
                Object.DontDestroyOnLoad(obj);
            }
            return _cameraManager;
        }
    }
    public static PlayerAudioManager _playerAudioManager;
    public static PlayerAudioManager PlayerAudio
    {
        get
        {
            if (_playerAudioManager == null)
            {
                GameObject obj = new GameObject("Player Audio Manager");
                _playerAudioManager = obj.AddComponent<PlayerAudioManager>();
                Object.DontDestroyOnLoad(obj);
            }
            return _playerAudioManager;
        }
    }
    public static EnemyAudioManager _enemyAudioManager;
    public static EnemyAudioManager EnemyAudio
    {
        get
        {
            if (_enemyAudioManager == null)
            {
                GameObject obj = new GameObject("Enemy Audio Manager");
                _enemyAudioManager = obj.AddComponent<EnemyAudioManager>();
                Object.DontDestroyOnLoad(obj);
            }
            return _enemyAudioManager;
        }
    }
    public static BGMAudioManager _bgmAudioManager;
    public static BGMAudioManager BGMAudio
    {
        get
        {
            if (_bgmAudioManager == null)
            {
                GameObject obj = new GameObject("BGM Audio Manager");
                _bgmAudioManager = obj.AddComponent<BGMAudioManager>();
                Object.DontDestroyOnLoad(obj);
            }
            return _bgmAudioManager;
        }
    }
}
