using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlow : MonoBehaviour
{
    [Header("������Ʈ Ǯ ����")]
    [SerializeField]
    private PoolingInfo[] poolingInfos;

    private void Start()
    {
        // ������Ʈ Ǯ ����
        foreach(PoolingInfo info in poolingInfos)
        {

            PoolManager.Instance.CreatePool(info.prefab, info.size, info.capacity);
        }
    }
}
