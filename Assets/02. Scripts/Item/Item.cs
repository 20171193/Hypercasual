using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 습득 아이템
/// </summary>
public abstract class Item : PooledObject
{
    public enum AcquireType
    {
        Stack,      // 스택형 아이템
        Consume     // 소모형 아이템
    }

    [Header("-Specs")]
    [Tooltip("아이템 모델 렌더러")]
    [SerializeField]
    protected Renderer itemRenderer;
    public Renderer ItemRenderer { get { return itemRenderer; } }

    [Tooltip("아이템 획득 타입")]
    [SerializeField]
    protected AcquireType acquireType = AcquireType.Consume;
    public AcquireType GetAcquireType { get { return acquireType; } }

    public abstract void GetItem();
}
