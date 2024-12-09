using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Progress;

/// <summary>
/// 크로아상 아이템
/// </summary>
public class Croassant : StackItem
{
    [Header("-Components")]
    [SerializeField]
    private Rigidbody rigid;
    public Rigidbody Rigidbody {get { return rigid; } }

    [SerializeField]
    private CapsuleCollider col;

    public override void OnGetItem()
    {
        // 물리 계산, 충돌체 해제
        rigid.isKinematic = true;
        col.enabled = false;
        base.OnGetItem();
    }
}
