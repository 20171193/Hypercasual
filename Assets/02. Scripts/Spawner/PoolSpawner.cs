using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ǯ���� ������ ������
/// </summary>
public class PoolSpawner<TObject> : MonoBehaviour where TObject : PooledObject
{
    [Header("-Components")]
    [SerializeField]
    protected TObject objectPrefab;

    [Space(10)]
    [Header("-Specs")]
    [Tooltip("Ǯ ������")]
    [SerializeField]
    private int poolSize;
    [Tooltip("Ǯ �뷮")]
    [SerializeField]
    private int poolCapacity;

    //[Space(10)]
    //[Header("-Ballancing")]
    //[Tooltip("������ ������ �ν��Ͻ� ID")]
    private Coroutine initDelay;
    
    private void OnEnable()
    {
        initDelay = StartCoroutine(InitDelay());
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

    IEnumerator InitDelay()
    {
        yield return new WaitForSeconds(0.1f);
        // ������Ʈ Ǯ ������û
        PoolManager.Instance.CreatePool(objectPrefab, poolSize, poolCapacity);
    }
}
