using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������Ʈ Ǯ �ֻ��� Ŭ����
/// </summary>
public class ObjectPool : MonoBehaviour
{
    [Header("-Specs")]
    [Tooltip("��� ������Ʈ")]
    [SerializeField]
    protected PooledObject prefab;
    [Tooltip("Ǯ ũ��")]
    [SerializeField]
    protected int size;
    [Tooltip("Ǯ �뷮")]
    [SerializeField]
    protected int capacity;

    [Space(10)]
    [Header("-Debugging")]
    [SerializeField]
    protected Queue<PooledObject> objectPool;

    // Ǯ ����
    public void CreatePool(PooledObject prefab, int size, int capacity)
    {
        this.prefab = prefab;
        this.size = size;
        this.capacity = capacity;
            
        // ������Ʈ �ε�
        for(int i =0; i<size; i++)
        {
            PooledObject inst = Instantiate(prefab);
            inst.gameObject.SetActive(false);
            inst.Pooler = this;
            inst.transform.parent = transform;
            objectPool.Enqueue(inst);
        }
    }
    // Ǯ �뿩
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
        // ��� ������Ʈ�� ����� ��� �߰� ����
        else
        {
            PooledObject inst = Instantiate(prefab);
            inst.Pooler = this;
            inst.transform.position = position;
            inst.transform.rotation = rotation;
            return inst;
        }
    }
    // Ǯ �ݳ�
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
        // �뷮 �̻����� ������ ������Ʈ�� ����
        else
        {
            Destroy(inst.gameObject);
        }
    }
}
