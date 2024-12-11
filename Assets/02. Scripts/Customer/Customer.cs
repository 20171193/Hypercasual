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

    // ���¸ӽ�
    private StateMachine<Customer> fsm;
    public StateMachine<Customer> FSM { get { return fsm; } }

    [Header("-Ballancing")]
    public Vector3 destination;

    [SerializeField]
    private Vector3 spawnPosition; 

    [SerializeField]
    private CustomerOrder ownOrder;
    public CustomerOrder OwnOrder { get { return ownOrder; } }  

    // ������ �Ÿ� ���� ��
    private float distanceOffset = 1f;

    private int paramID_IsMoving = -1;
    public int ParamID_IsMoving {get { return paramID_IsMoving; } }

    private int paramID_IsStacking = -1;
    public int ParamID_IsStacking { get { return paramID_IsStacking; } }

    // ���� ��⿭ ����
    public int orderTurn = -1;

    public string CurState;

    private void Awake()
    {
        fsm = new StateMachine<Customer>(this);
        fsm.AddState("Pooled", new Pooled(this));
        fsm.AddState("Selecting", new Selecting(this));
        fsm.AddState("Packaging", new Packaging(this));
        fsm.AddState("DineIn", new DineIn(this));
        fsm.AddState("SuccessOrder", new SuccessOrder(this));
        
        paramID_IsMoving = Animator.StringToHash("IsMoving");
        paramID_IsStacking = Animator.StringToHash("IsStacking");
    }

    private void OnEnable()
    {
        fsm.Init("Pooled");
        ownOrder = OrderManager.Instance.GetOrder();
    }

    private void OnDisable()
    {
        orderTurn = -1;

        // �ִϸ����� �ʱ�ȭ
        anim.SetBool(paramID_IsStacking, false);
        anim.SetBool(paramID_IsMoving, false);
    }

    private void Update()
    {
        CurState = fsm.CurState;
        fsm.Update();
    }

    // ������ ���� : Ÿ ��ü ȣ�� (�÷��̾�, ���̺�)
    public void SendItem(Item item)
    {
        itemController.ItemStack.PushItem(item);
        
        switch(CurState)
        {
            // �� ���� 
            case "Selecting":
                // ��� ������ ������ �Ϸ�� ���
                if (itemController.ItemStack.CurStackCount == ownOrder.orderCount)
                {
                    // �ֹ� Ÿ���� ������ ��� Packaging ���·� ���� 
                    if (ownOrder.orderType == OrderType.TakeOut)
                    {
                        // ������ ���� : ī����
                        destination = OrderManager.Instance.counter.GetWatingLine(this);
                        fsm.ChangeState("Packaging");
                    }
                    // �ֹ� Ÿ���� ������ ��� DineIn ���·� ����
                    else if(ownOrder.orderType == OrderType.DineIn)
                    {
                        // ������ ���� : ���� ���̺� ��⿭
                        destination = OrderManager.Instance.dineIn.GetWaitingLine(this);
                        fsm.ChangeState("DineIn");
                    }
                }
                break;
            // ������ ����
            case "Packaging":
                OrderManager.Instance.counter.PayMoney(ownOrder.orderCount * 5);

                // ������ ���� : ���Ա�
                destination = OrderManager.Instance.entranceTr.position;
                // SuccessOrder ���� ���� -> Release
                fsm.ChangeState("SuccessOrder");
                break;
        }
    }
}
