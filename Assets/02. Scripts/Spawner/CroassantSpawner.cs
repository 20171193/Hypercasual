using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ũ�ξƻ� ������
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
        inst.ChangeState(Croassant.State.OnSpawner);
    }

}
