using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 상호작용 테이블 추상 클래스
/// </summary>
public abstract class Table : MonoBehaviour
{
    /// <summary>
    /// 상호작용 타입
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
