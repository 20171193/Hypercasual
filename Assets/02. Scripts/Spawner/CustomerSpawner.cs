using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : PoolSpawner<Customer>
{
    public override PooledObject Spawn()
    {
        PooledObject inst = base.Spawn();
        inst.transform.position = transform.position;
        return inst;
    }
}
