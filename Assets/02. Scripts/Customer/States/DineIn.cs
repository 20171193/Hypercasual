using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DineIn : CustomerState
{
    public DineIn(Customer owner)
    {
        this.owner = owner;
    }

    // ������ Ȯ�� �ڷ�ƾ
    private Coroutine checkDestinationRoutine;

    // �Ļ� ������
    private Coroutine eatingDelay;

    public override void Enter()
    {
        owner.Agent.isStopped = false;
        owner.Agent.destination = owner.destination;
        checkDestinationRoutine = owner.StartCoroutine(CheckDestinationRoutine());
        // �ִϸ����� ������Ʈ
        owner.Anim.SetBool(owner.ParamID_IsMoving, true);

        // ��⿭�� ��ϵ� ��� �̺�Ʈ ��� (�ٴ��)
        if (owner.orderTurn >= 0)
            OrderManager.Instance.dineIn.OnTableEnable.AddListener(ShiftLine);
    }

    public override void Exit()
    {
        // �̺�Ʈ ���� (�ٴ��)
        OrderManager.Instance.dineIn.OnTableEnable.RemoveListener(ShiftLine);

        checkDestinationRoutine = null;
    }

    // �� ����
    private void ShiftLine()
    {
        // ������ �缳��
        owner.destination = OrderManager.Instance.dineIn.GetWaitingLine(--owner.orderTurn);

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
        // ��Ű¡�� ������ ��� ��Ű¡ ��û
        if (owner.orderTurn < 0)
        {
            Debug.Log("��Ű¡ ��û");
            // ���� �Ҵ� (���̺� ����)
            owner.transform.forward = OrderManager.Instance.dineIn.TableTr.forward;
            eatingDelay = owner.StartCoroutine(EatingDelay());
        }
        else
        {
            // ���� �Ҵ� (ī���� ����)
            owner.transform.forward = (OrderManager.Instance.counter.transform.position - owner.transform.position).normalized;
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

    private IEnumerator EatingDelay()
    {
        // ������ �� ����
        owner.ItemController.ItemStack.PopItem().Release();
        // �� ����
        Croassant croassant = OrderManager.Instance.dineIn.SpawnCroassant();
        yield return new WaitForSeconds(3f);
        croassant.Release();
        // ������ ����
        OrderManager.Instance.dineIn.SpawnTrash();

        // ������ ����
        owner.destination = OrderManager.Instance.entranceTr.position;
        // ���� ����
        owner.FSM.ChangeState("SuccessOrder");
    }
}
