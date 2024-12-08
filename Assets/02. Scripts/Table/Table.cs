using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 상호작용 테이블 추상 클래스
/// </summary>

public abstract class Table : MonoBehaviour
{
    /// <summary>
    /// 테이블 타입
    /// </summary>
    public enum TableType
    {
        Oven = 0,
        Basket,
        Counter
    }

    private TableType type;
    public TableType Type { get { return type; } }

    public abstract void Interact(ItemStack itemStack);
}
