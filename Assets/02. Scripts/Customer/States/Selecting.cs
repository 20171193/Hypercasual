using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// �� �����ҷ� �̵� �� �ֹ� ������
/// </summary>
public class Selecting : CustomerState
{
    public Selecting(Customer owner)
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
    }
    public override void Exit()
    {
        checkDestinationRoutine = null;
    }

    // �� �����ҿ� ������ ���
    private void Arrived()
    {
        // �ִϸ����� ������Ʈ (�ȱ� ��Ȱ��ȭ)
        owner.Anim.SetBool(owner.ParamID_IsMoving, false);
        owner.Agent.isStopped = true;
        owner.Agent.transform.position = owner.destination;

        // �� ������ ���� ȸ��
        owner.transform.forward = OrderManager.Instance.basket.transform.position;

        // �� �ֹ���û
        OrderManager.Instance.basket.RequestOrder(owner, owner.OwnOrder.orderCount);
    }

    // ������ Ȯ�� ��ƾ
    private IEnumerator CheckDestinationRoutine()
    {
        while((owner.destination - owner.transform.position).sqrMagnitude > 0.5f)
        {
            yield return new WaitForSeconds(0.2f);
        }
        // �������� ������ ���
        Arrived();
    }
}
