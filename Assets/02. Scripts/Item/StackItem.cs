using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ������ ������
/// </summary>
public class StackItem : Item
{
    public enum State
    {
        Pooled,             // ������Ʈ Ǯ�� ������� ���� (�ʱ�ȭ)
        //OnSpawner,          //                 ������ ���� (������ ����)
        OnStack,            //            ���ÿ� ���� ���� (������ ����) 
    }

    [Header("-Ballancing")]
    [SerializeField]
    protected State curState = State.Pooled;
    public State CurState { get { return curState; }}

    public UnityEvent OnStacked;

    protected virtual void Awake()
    {
        acquireType = AcquireType.Stack;
    }

    // ������ ������ ���� ����
    public void ChangeState(State state)
    {
        switch (state)
        {
            case State.Pooled:
                {
                    pooler.ReturnPool(this);
                    break;
                }
            case State.OnStack:
                {
                    OnStacked?.Invoke();
                    break;
                }
            default:
                break;
        }
        curState = state;
    }

    public override void OnGetItem()
    {
        ChangeState(State.OnStack);
    }
}
