using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �� ������ ����
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

            // �ִϸ����� ������Ʈ
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
