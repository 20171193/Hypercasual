using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 스택형 아이템
/// </summary>
public class StackItem : Item
{
    public enum State
    {
        Pooled,             // 오브젝트 풀에 대기중인 상태 (초기화)
        //OnSpawner,          //                 생성된 상태 (물리력 적용)
        OnStack,            //            스택에 쌓인 상태 (물리력 해제) 
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

    // 스택형 아이템 상태 적용
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
