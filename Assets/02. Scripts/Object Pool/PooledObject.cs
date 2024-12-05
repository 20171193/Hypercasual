using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// 풀링 정보 
[Serializable]
public class PoolingInfo
{
    public PooledObject prefab;
    public int size;
    public int capacity;
}

/// <summary>
/// 풀링 오브젝트 최상위 클래스
/// </summary>
public class PooledObject : MonoBehaviour
{
    [SerializeField]
    protected ObjectPool pooler;
    public ObjectPool Pooler { set { pooler = value; } }

    // 오브젝트 반납 (풀 반납)
    public virtual void Release()
    {
        if(pooler != null)
        {
            pooler.ReturnPool(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
