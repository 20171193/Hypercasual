using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : PoolSpawner<Customer>
{
    [ContextMenu("Spawn")]
    protected override void Spawn()
    {
        Customer inst = PoolManager.Instance.GetPool(objectPrefab, Vector3.zero, Quaternion.identity) as Customer;
        if (inst == null)
        {
            Debug.Log("Ǯ�� ��ϵ��� ���� ������Ʈ : Customer");
            return;
        }
    }
}
