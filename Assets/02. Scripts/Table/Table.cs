using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ȣ�ۿ� ���̺� �߻� Ŭ����
/// </summary>
public abstract class Table : MonoBehaviour
{
    /// <summary>
    /// ��ȣ�ۿ� Ÿ��
    /// </summary>
    public enum InteractType
    {
        Player = 0,
        Customer,
        Both
    }

    private InteractType interactType;
    public InteractType GetInteractType {get { return interactType; } }

    public abstract bool InteractStack(ItemStack targetStack);
}
