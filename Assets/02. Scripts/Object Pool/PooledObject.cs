using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Ǯ�� ���� 
[Serializable]
public class PoolingInfo
{
    public PooledObject prefab;
    public int size;
    public int capacity;
}

/// <summary>
/// Ǯ�� ������Ʈ �ֻ��� Ŭ����
/// </summary>
public class PooledObject : MonoBehaviour
{
    [SerializeField]
    protected ObjectPool pooler;
    public ObjectPool Pooler { set { pooler = value; } }

    // ������Ʈ �ݳ� (Ǯ �ݳ�)
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
