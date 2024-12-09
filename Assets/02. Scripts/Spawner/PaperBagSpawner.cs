using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperBagSpawner : PoolSpawner<PaperBag>
{
    public override PooledObject Spawn()
    {
        PooledObject inst = base.Spawn();
        // 포장용지 위치 세팅
        inst.transform.position = transform.position;
        inst.transform.forward = transform.forward;
        return inst;
    }
}
