using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperBag : Item
{
    [Header("-Components")]
    [SerializeField]
    private Animator anim;

    private int paramID_IsPackaged = -1;

    private void Awake()
    {
        paramID_IsPackaged = Animator.StringToHash("IsPackaged");
    }

    private void OnEnable()
    {
        // �ִϸ����� �ʱ�ȭ
        anim.SetBool(paramID_IsPackaged, false);
    }

    public override void OnGetItem()
    {
        anim.SetBool(paramID_IsPackaged, true);
    }
}
