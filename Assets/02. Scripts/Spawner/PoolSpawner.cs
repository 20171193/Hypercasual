using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ǯ���� ������ ������
/// </summary>
public class PoolSpawner<TObject> : MonoBehaviour where TObject : PooledObject
{
    [SerializeField]
    protected TObject objectPrefab;

    [Tooltip("������ ������ �ν��Ͻ� ID")]
    [SerializeField]
    protected int instanceID;
    public int InstanceID { get { return instanceID; } }

    private void Awake()
    {
        // ������Ʈ Ǯ�� ���
        instanceID = objectPrefab.GetInstanceID();
    }

    protected virtual PooledObject SpawnItem()
    {
        PooledObject inst = PoolManager.Instance.GetPool(objectPrefab, Vector3.zero, Quaternion.identity) as Item;
        if (inst == null)
        {
            Debug.Log($"Ǯ������ ���� ������ : {objectPrefab}");
            return null;
        }
        return inst;
    }
}
