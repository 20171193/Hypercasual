using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� ������
/// </summary>
public class Item : PooledObject
{
    [Tooltip("������ �� ������")]
    [SerializeField]
    private Renderer itemRenderer;
    public Renderer ItemRenderer { get { return itemRenderer; } }

    public Item GetItem()
    {


        return this;
    }

}
