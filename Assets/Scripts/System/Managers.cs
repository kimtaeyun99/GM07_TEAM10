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
    //private static CameraManager _cameraManager;

    //public static CameraManager Camera
    //{
    //    get
    //    {
    //        if(_cameraManager == null)
    //        {
    //            GameObject obje = new GameObject(Camera)
    //        }
    //    }
    //}
}
