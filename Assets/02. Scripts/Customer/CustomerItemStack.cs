using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 고객 아이템 스택
/// </summary>
public class CustomerItemStack : ItemStack
{
    [Header("-Components")]
    [SerializeField]
    private Animator anim;

    public override int CurStackCount 
    { 
        set
        {
            base.CurStackCount = value;

            // 애니메이터 업데이트
            if (value == 0)
                anim.SetBool(paramID_IsStacking, false);
            else
                anim.SetBool(paramID_IsStacking, true);
        }
        get { return base.CurStackCount; }
    }

    private int paramID_IsStacking = -1;

    protected override void Awake()
    {
        base.Awake();
        paramID_IsStacking = Animator.StringToHash("IsStacking");
    }

}
