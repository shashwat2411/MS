using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private static ObjectPool instance;
    private Dictionary<string, Queue<GameObject>> objectPool = new Dictionary<string, Queue<GameObject>>();
   
    private GameObject pool = null;
    public static ObjectPool Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ObjectPool();
            }
            return instance;
        }
    }

    public GameObject Get(GameObject prefab,Vector3 pos,Quaternion rot)
    {
        GameObject obj;
        if (!objectPool.ContainsKey(prefab.name) || objectPool[prefab.name].Count == 0)
        {
            obj = GameObject.Instantiate(prefab);
            Push(obj);
            if (pool == null)
            {
                pool = new GameObject("ObjectPool");
            }
      
            GameObject childPool = GameObject.Find("ObjectPool/" + prefab.name + "Pool");
            if (!childPool)
            {
                childPool = new GameObject(prefab.name + "Pool");
                childPool.transform.SetParent(pool.transform);
            }
            obj.transform.SetParent(childPool.transform);
        }

        obj = objectPool[prefab.name].Dequeue();
        obj.transform.position = pos;
        obj.transform.rotation = rot;

        obj.SetActive(true);
        return obj;
    }

    public void Push(GameObject prefab)
    {
        string name = prefab.name.Replace("(Clone)", string.Empty);
        if (!objectPool.ContainsKey(name))
        {
            objectPool.Add(name, new Queue<GameObject>());
        }
        objectPool[name].Enqueue(prefab);
        prefab.SetActive(false);
    }
}
