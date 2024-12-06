using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 풀링된 아이템 생성기
/// </summary>
public class PoolSpawner<TObject> : MonoBehaviour where TObject : PooledObject
{
    [SerializeField]
    protected TObject objectPrefab;

    [Tooltip("아이템 프리팹 인스턴스 ID")]
    [SerializeField]
    protected int instanceID;
    public int InstanceID { get { return instanceID; } }

    private void Awake()
    {
        // 오브젝트 풀에 등록
        instanceID = objectPrefab.GetInstanceID();
    }

    protected virtual PooledObject SpawnItem()
    {
        PooledObject inst = PoolManager.Instance.GetPool(objectPrefab, Vector3.zero, Quaternion.identity) as Item;
        if (inst == null)
        {
            Debug.Log($"풀링되지 않은 프리팹 : {objectPrefab}");
            return null;
        }
        return inst;
    }
}
