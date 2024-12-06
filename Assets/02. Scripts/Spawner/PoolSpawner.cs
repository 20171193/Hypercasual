using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 풀링된 아이템 생성기
/// </summary>
public class PoolSpawner<TObject> : MonoBehaviour where TObject : PooledObject
{
    [Header("-Components")]
    [SerializeField]
    protected TObject objectPrefab;

    [Space(10)]
    [Header("-Specs")]
    [Tooltip("풀 사이즈")]
    [SerializeField]
    private int poolSize;
    [Tooltip("풀 용량")]
    [SerializeField]
    private int poolCapacity;

    //[Space(10)]
    //[Header("-Ballancing")]
    //[Tooltip("아이템 프리팹 인스턴스 ID")]
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
            Debug.Log($"풀링되지 않은 프리팹 : {objectPrefab}");
            return null;
        }
        return inst;
    }

    IEnumerator InitDelay()
    {
        yield return new WaitForSeconds(0.1f);
        // 오브젝트 풀 생성요청
        PoolManager.Instance.CreatePool(objectPrefab, poolSize, poolCapacity);
    }
}
