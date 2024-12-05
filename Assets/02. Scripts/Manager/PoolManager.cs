using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

// 오브젝트 풀 매니저
public class PoolManager : Singleton<PoolManager>
{
    /// <summary>
    /// 인스턴스ID, 풀 인스턴스
    /// </summary>
    private Dictionary<int, ObjectPool> poolDic = new Dictionary<int, ObjectPool>();

    public void CreatePool(PooledObject prefab, int size, int capacity)
    {
        // 풀 생성 (각 오브젝트들이 위치하는 부모 오브젝트)
        GameObject gameObject = new GameObject();
        gameObject.name = $"Pool_{prefab.name}";

        ObjectPool objectPool = gameObject.AddComponent<ObjectPool>();
        objectPool.CreatePool(prefab, size, capacity);

        poolDic.Add(prefab.GetInstanceID(), objectPool);
    }

    public void DestroyPool(PooledObject prefab)
    {
        ObjectPool objectPool = poolDic[prefab.GetInstanceID()];
        Destroy(objectPool.gameObject);

        poolDic.Remove(prefab.GetInstanceID());
    }

    public void ClearPool()
    {
        // 풀 비우기
        foreach (ObjectPool objectPool in poolDic.Values)
        {
            Destroy(objectPool.gameObject);
        }

        poolDic.Clear();
    }

    public PooledObject GetPool(PooledObject prefab, Vector3 position, Quaternion rotation)
    {
        // 프리팹의 InstanceID로 로딩
        return poolDic[prefab.GetInstanceID()].GetPool(position, rotation);
    }
}
