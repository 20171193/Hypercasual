using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Customer : PooledObject
{
    [Header("-Components")]
    [SerializeField]
    private NavMeshAgent agent;
    public NavMeshAgent Agent { get { return agent; } }

    [SerializeField]
    private Animator anim;
    public Animator Anim {get { return anim; } }

    [SerializeField]
    private CustomerItemController itemController;
    public CustomerItemController ItemController { get { return itemController; } } 

    // 상태머신
    private StateMachine<Customer> fsm;
    public StateMachine<Customer> FSM { get { return fsm; } }

    [Header("-Ballancing")]
    [SerializeField]
    public Vector3 destination;

    private CustomerOrder ownOrder;
    public CustomerOrder OwnOrder { get { return ownOrder; } }  

    // 목적지 거리 보정 값
    private float distanceOffset = 1f;

    private int paramID_IsMoving = -1;
    public int ParamID_IsMoving {get { return paramID_IsMoving; } }

    private int paramID_IsStacking = -1;
    public int ParamID_IsStacking { get { return paramID_IsStacking; } }

    private void Awake()
    {
        fsm = new StateMachine<Customer>(this);
        fsm.AddState("Pooled", new Pooled(this));
        fsm.AddState("Selecting", new Selecting(this));
        fsm.AddState("Packaging", new Packaging(this));
        fsm.AddState("DineIn", new DineIn(this));
        fsm.AddState("SuccessOrder", new SuccessOrder(this));
        fsm.Init("Pooled");

        paramID_IsMoving = Animator.StringToHash("IsMoving");
        paramID_IsStacking = Animator.StringToHash("IsStacking");
    }
    private void OnEnable()
    {
        
    }
    private void Update()
    {
        fsm.Update();
    }


    // 아이템 전달 : 타 객체 호출 (플레이어, 테이블)
    public void SendItem(Item croassant)
    {
        itemController.ItemStack.PushItem(croassant);

        // 모든 아이템 전달이 완료된 경우
        if (itemController.ItemStack.isFull)
        {
            // 주문 타입이 포장일 경우 Packaging 상태로 전이 
            if (ownOrder.orderType == OrderType.TakeOut)
            {
                fsm.ChangeState("Packaging");
            }
        }
    }
}
