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
            Debug.Log("풀에 등록되지 않은 오브젝트 : Customer");
            return;
        }
    }
}
