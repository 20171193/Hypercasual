using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlow : MonoBehaviour
{
    [Header("오브젝트 풀 정보")]
    [SerializeField]
    private PoolingInfo[] poolingInfos;

    private void Start()
    {
        // 오브젝트 풀 생성
        foreach(PoolingInfo info in poolingInfos)
        {

            PoolManager.Instance.CreatePool(info.prefab, info.size, info.capacity);
        }
    }
}
