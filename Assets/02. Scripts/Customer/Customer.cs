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

    [SerializeField]
    private CustomerOrder ownOrder;
    public CustomerOrder OwnOrder { get { return ownOrder; } }  

    // 목적지 거리 보정 값
    private float distanceOffset = 1f;

    private int paramID_IsMoving = -1;
    public int ParamID_IsMoving {get { return paramID_IsMoving; } }

    private int paramID_IsStacking = -1;
    public int ParamID_IsStacking { get { return paramID_IsStacking; } }

    // 현재 대기열 순서
    public int orderTurn = -1;

    private string CurState => fsm.CurState;

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
        ownOrder = OrderManager.Instance.GetOrder();
    }

    private void OnDisable()
    {
        orderTurn = -1;
    }

    private void Update()
    {
        fsm.Update();
    }

    // 아이템 전달 : 타 객체 호출 (플레이어, 테이블)
    public void SendItem(Item item)
    {
        itemController.ItemStack.PushItem(item);
        
        switch(CurState)
        {
            // 빵 전달 
            case "Selecting":
                // 모든 아이템 전달이 완료된 경우
                if (itemController.ItemStack.CurStackCount == ownOrder.orderCount)
                {
                    // 주문 타입이 포장일 경우 Packaging 상태로 전이 
                    if (ownOrder.orderType == OrderType.TakeOut)
                    {
                        // 목적지 설정 : 카운터
                        destination = OrderManager.Instance.counter.GetWatingLine(this);
                        fsm.ChangeState("Packaging");
                    }
                }
                break;
            // 포장용기 전달
            case "Packaging":
                break;
        }
    }
}
