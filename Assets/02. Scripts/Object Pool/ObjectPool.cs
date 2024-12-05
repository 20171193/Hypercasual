using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 오브젝트 풀 최상위 클래스
/// </summary>
public class ObjectPool : MonoBehaviour
{
    [Header("-Specs")]
    [Tooltip("대상 오브젝트")]
    [SerializeField]
    protected PooledObject prefab;
    [Tooltip("풀 크기")]
    [SerializeField]
    protected int size;
    [Tooltip("풀 용량")]
    [SerializeField]
    protected int capacity;

    [Space(10)]
    [Header("-Debugging")]
    [SerializeField]
    protected Queue<PooledObject> objectPool;

    // 풀 생성
    public void CreatePool(PooledObject prefab, int size, int capacity)
    {
        this.prefab = prefab;
        this.size = size;
        this.capacity = capacity;
            
        // 오브젝트 로딩
        for(int i =0; i<size; i++)
        {
            PooledObject inst = Instantiate(prefab);
            inst.gameObject.SetActive(false);
            inst.Pooler = this;
            inst.transform.parent = transform;
            objectPool.Enqueue(inst);
        }
    }
    // 풀 대여
    public PooledObject GetPool(Vector3 position, Quaternion rotation)
    {
        if(objectPool.Count > 0)
        {
            PooledObject inst = objectPool.Dequeue();
            inst.transform.position = position;
            inst.transform.rotation = rotation;
            inst.gameObject.SetActive(true);
            return inst;
        }
        // 모든 오브젝트를 사용한 경우 추가 생성
        else
        {
            PooledObject inst = Instantiate(prefab);
            inst.Pooler = this;
            inst.transform.position = position;
            inst.transform.rotation = rotation;
            return inst;
        }
    }
    // 풀 반납
    public void ReturnPool(PooledObject inst)
    {
        if (inst == null)
            return;

        if(objectPool.Count < capacity) 
        {
            inst.gameObject.SetActive(false);
            inst.transform.parent = transform;
            objectPool.Enqueue(inst);
        }
        // 용량 이상으로 생성한 오브젝트는 제거
        else
        {
            Destroy(inst.gameObject);
        }
    }
}
