using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아이템 스택과 상호작용 (Table)
/// </summary>
public interface IStackInteractable
{
    public bool InteractStack(ItemStack stack);
}
