using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ω¿µÊ æ∆¿Ã≈€
/// </summary>
public class Item : PooledObject
{
    [Tooltip("æ∆¿Ã≈€ ∏µ® ∑ª¥ı∑Ø")]
    [SerializeField]
    private Renderer itemRenderer;
    public Renderer ItemRenderer { get { return itemRenderer; } }

    public Item GetItem()
    {


        return this;
    }

}
