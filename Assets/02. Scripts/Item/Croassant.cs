using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Progress;

/// <summary>
/// ũ�ξƻ� ������
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
        // ���� ���, �浹ü ����
        rigid.isKinematic = true;
        col.enabled = false;
        base.OnGetItem();
    }
}
