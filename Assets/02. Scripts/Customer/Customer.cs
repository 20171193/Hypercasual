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
    [SerializeField]
    public Vector3 destination;

    private CustomerOrder ownOrder;
    public CustomerOrder OwnOrder { get { return ownOrder; } }  

    // ������ �Ÿ� ���� ��
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


    // ������ ���� : Ÿ ��ü ȣ�� (�÷��̾�, ���̺�)
    public void SendItem(Item croassant)
    {
        itemController.ItemStack.PushItem(croassant);

        // ��� ������ ������ �Ϸ�� ���
        if (itemController.ItemStack.isFull)
        {
            // �ֹ� Ÿ���� ������ ��� Packaging ���·� ���� 
            if (ownOrder.orderType == OrderType.TakeOut)
            {
                fsm.ChangeState("Packaging");
            }
        }
    }
}
