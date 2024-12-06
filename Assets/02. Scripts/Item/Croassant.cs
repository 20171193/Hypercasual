using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 크로아상 아이템
/// </summary>
public class Croassant : Item
{
    // 크로아상 상태
    public enum State
    {
        Pooled,             // 오브젝트 풀에 대기중인 상태 (초기화)
        OnSpawner,          //                 생성된 상태 (물리력 적용)
        OnPlayerStack,      //   플레이어 스택에 쌓인 상태 (물리력 해제) 
        OnCustomerStack,    //       고객 스택에 쌓인 상태 (물리력 해제)
        OnBasketStack       //     보관대 스택에 쌓인 상태 (물리력 해제)
    }

    [Header("-Components")]
    [SerializeField]
    private Rigidbody rigid;

    [Header("-Specs")]
    [Tooltip("스폰될 때 튀어나가는 힘")]
    [SerializeField]
    private float spawnPower;

    [Header("-Ballancing")]
    [SerializeField]
    private State curState = State.Pooled;
    public State CurState { get { return curState; } }

    public void ChangeState(State state)
    {
        switch (state)
        {
            case State.Pooled:
                {
                    // 물리계산 정지
                    rigid.isKinematic = false;
                    pooler.ReturnPool(this);
                    break;
                }
            case State.OnSpawner:
                {
                    // 물리계산 실행
                    rigid.isKinematic = false;
                    // 스폰 시 튀어나가는 힘 적용
                    rigid.AddForce(transform.forward * spawnPower, ForceMode.Impulse);
                    break;
                }
            case State.OnPlayerStack:
            case State.OnCustomerStack:
            case State.OnBasketStack:
                {
                    // 물리계산 정지
                    rigid.isKinematic = true;
                    break;
                }
            default:
                break;
        }

        curState = state;
    }
}
