using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

// ������Ʈ Ǯ �Ŵ���
public class PoolManager : Singleton<PoolManager>
{
    /// <summary>
    /// �ν��Ͻ�ID, Ǯ �ν��Ͻ�
    /// </summary>
    private Dictionary<int, ObjectPool> poolDic = new Dictionary<int, ObjectPool>();

    public void ExtendPool(PooledObject prefab, int capacity)
    {
        int prefabID = prefab.GetInstanceID();
        if (!poolDic.ContainsKey(prefabID))
            return;

        // Ǯ ũ�� Ȯ��
        poolDic[prefabID].ExtendPool(capacity);
    }
    public void CreatePool(PooledObject prefab, int size, int capacity)
    {
        // Ǯ ���� (�� ������Ʈ���� ��ġ�ϴ� �θ� ������Ʈ)
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
        // Ǯ ����
        foreach (ObjectPool objectPool in poolDic.Values)
        {
            Destroy(objectPool.gameObject);
        }

        poolDic.Clear();
    }
    public PooledObject GetPool(PooledObject prefab, Vector3 position, Quaternion rotation)
    {
        // �������� InstanceID�� �ε�
        return poolDic[prefab.GetInstanceID()].GetPool(position, rotation);
    }

}
