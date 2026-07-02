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
            return _playerAudioManager;
        }
    }
    public static EnemyAudioManager _enemyAudioManager;
    public static EnemyAudioManager EnemyAudio
    {
        get
        {
            return _enemyAudioManager;
        }
    }
    public static BGMAudioManager _bgmAudioManager;
    public static BGMAudioManager BGMAudio
    {
        get
        {
            return _bgmAudioManager;
        }
    }
}
