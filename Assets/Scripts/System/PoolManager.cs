using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private Transform poolRoot;

    private Dictionary<int, Queue<Component>> poolDictionary = new Dictionary<int, Queue<Component>>();
    private Dictionary<int, Transform> poolParents = new Dictionary<int, Transform>();
    private Dictionary<Component, int> objectToPoolKey = new Dictionary<Component, int>();
    private void Awake()
    {
        CreatePoolRoot();
    }
    private void CreatePoolRoot()
    {
        GameObject rootObj = new GameObject("PoolRoot");

        rootObj.transform.SetParent(transform);

        poolRoot = rootObj.transform;
    }
    private void CreatePool(int key, string prefabName)
    {
        if (poolDictionary.ContainsKey(key)) return;

        poolDictionary.Add(key, new Queue<Component>());

        CreatePoolParent(key, prefabName);
    }
    private void CreatePoolParent(int key, string prefabName)
    {
        GameObject parentObj = new GameObject($"{prefabName}_{key}");
        parentObj.transform.SetParent(poolRoot);
        poolParents.Add(key, parentObj.transform);
    }
    public void PreloadPool<T>(T prefab, int count) where T : Component
    {
        int key = prefab.GetInstanceID();

        CreatePool(key,prefab.name);

        for(int i=0; i< count; i++)
        {
            T obj = Instantiate(prefab);

            obj.gameObject.SetActive(false);

            obj.transform.SetParent(poolParents[key]);

            objectToPoolKey[obj] = key;

            poolDictionary[key].Enqueue(obj);
        }
    }
    public T GetPool<T>(T prefab) where T : Component
    {
        int key = prefab.GetInstanceID();

        CreatePool(key,prefab.name);

        T obj = null;

        if (poolDictionary[key].Count >0)
        {
            obj = poolDictionary[key].Dequeue() as T;
        }
        else
        {
            obj = Instantiate(prefab);

            obj.transform.SetParent(poolParents[key]);

            objectToPoolKey[obj] = key;
        }

        obj.gameObject.SetActive(true);

        return obj;
    }
    public void ReturnPool<T>(T obj) where T : Component
    {
        if (!objectToPoolKey.TryGetValue(obj, out int key))
        {
            Destroy(obj.gameObject);
            return;
        }

        obj.gameObject.SetActive(false);

        if(obj.transform.parent != poolParents[key])
        {
            obj.transform.SetParent(poolParents[key]);
        }
        poolDictionary[key].Enqueue(obj);
    }
}
