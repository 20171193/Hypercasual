using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ω¿µÊ æ∆¿Ã≈€
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

    [Tooltip("æ∆¿Ã≈€ ∏µ® ∑ª¥ı∑Ø")]
    [SerializeField]
    private Renderer itemRenderer;
    public Renderer ItemRenderer { get { return itemRenderer; } }

}
