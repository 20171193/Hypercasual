using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� ������
/// </summary>

public enum ItemType
{
    Croassant = 0,
    Money = 1
}

public class Item : PooledObject
{
    [SerializeField]
    private ItemType itemType;
    public ItemType ItemType { get { return itemType; } }
    


}
