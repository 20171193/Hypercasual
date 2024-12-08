using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ȣ�ۿ� ���̺� �߻� Ŭ����
/// </summary>

public abstract class Table : MonoBehaviour
{
    /// <summary>
    /// ���̺� Ÿ��
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
