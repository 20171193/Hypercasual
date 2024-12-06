using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 크로아상 스포너
/// </summary>
public class CroassantSpawner : PoolSpawner<Item>
{
    private int maxSpawnCount = 0;
    public int MaxSpawnCount {get { return maxSpawnCount; } }

    private int curSpawnCount = 0;
    public int CurSpawnCount {get { return curSpawnCount; }}


    [ContextMenu("Spawn")]
    private void Spawn()
    {
        Croassant inst = base.SpawnItem() as Croassant;
        inst.transform.position = transform.position;
        inst.transform.rotation = transform.rotation;
        inst.transform.parent = transform;
        inst.ChangeState(Croassant.State.OnSpawner);
    }

}
