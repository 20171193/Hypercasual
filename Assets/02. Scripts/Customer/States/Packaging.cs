using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Packaging : CustomerState
{
    public Packaging(Customer owner)
    {
        this.owner = owner;
    }

    // ������ Ȯ�� �ڷ�ƾ
    private Coroutine checkDestinationRoutine;

    public override void Enter()
    {
        owner.Agent.isStopped = false;
        owner.Agent.destination = owner.destination;
        checkDestinationRoutine = owner.StartCoroutine(CheckDestinationRoutine());
        // �ִϸ����� ������Ʈ
        owner.Anim.SetBool(owner.ParamID_IsMoving, true);

        // ��⿭�� ��ϵ� ��� �̺�Ʈ ��� (�ٴ��)
        if (owner.orderTurn > 0)
            OrderManager.Instance.counter.OnProcessedOrder.AddListener(ShiftLine);
    }

    public override void Exit()
    {
        // �̺�Ʈ ���� (�ٴ��)
        OrderManager.Instance.counter.OnProcessedOrder.RemoveListener(ShiftLine);

        checkDestinationRoutine = null;
    }

    // �� ����
    private void ShiftLine()
    {
        // ������ �缳��
        owner.destination = OrderManager.Instance.counter.GetWatingLine(--owner.orderTurn);

        // �̵�
        owner.Agent.isStopped = false;
        owner.Agent.destination = owner.destination;

        // �ִϸ����� ������Ʈ
        owner.Anim.SetBool(owner.ParamID_IsMoving, true);

        checkDestinationRoutine = owner.StartCoroutine(CheckDestinationRoutine());
    }

    private void Arrived()
    {
        owner.Agent.isStopped = true;
        // �ִϸ����� ������Ʈ
        owner.Anim.SetBool(owner.ParamID_IsMoving, false);
        // ���� �Ҵ�
        owner.transform.forward = (OrderManager.Instance.counter.transform.position - owner.transform.position).normalized;
        // ��Ű¡�� ������ ��� ��Ű¡ ��û
        if (owner.orderTurn == 0)
        {
            Debug.Log("��Ű¡ ��û");
            OrderManager.Instance.counter.RequestOrder(owner);
        }
    }

    private IEnumerator CheckDestinationRoutine()
    {
        // ���������� ���� Ȯ��
        while ((owner.destination - owner.transform.position).sqrMagnitude > 0.5f)
        {
            yield return new WaitForSeconds(0.2f);
        }

        // �������� ������ ���
        Arrived();
    }
}
