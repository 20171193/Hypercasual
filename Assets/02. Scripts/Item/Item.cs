using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� ������
/// </summary>
public abstract class Item : PooledObject
{
    public enum AcquireType
    {
        Stack,      // ������ ������
        Consume     // �Ҹ��� ������
    }

    [Header("-Specs")]
    [Tooltip("������ �� ������")]
    [SerializeField]
    protected Renderer itemRenderer;
    public Renderer ItemRenderer { get { return itemRenderer; } }

    [Tooltip("������ ȹ�� Ÿ��")]
    [SerializeField]
    protected AcquireType acquireType = AcquireType.Consume;
    public AcquireType GetAcquireType { get { return acquireType; } }

    public abstract void GetItem();
}
