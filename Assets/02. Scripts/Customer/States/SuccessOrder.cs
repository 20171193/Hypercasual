using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuccessOrder : CustomerState
{
    public SuccessOrder(Customer owner)
    {
        this.owner = owner;
    }

    // ������ Ȯ�� �ڷ�ƾ
    private Coroutine checkDestinationRoutine;

    public override void Enter()
    {
        Debug.Log("Enter Success Order");
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
    private void Arrived()
    {
        owner.Agent.isStopped = true;
        // �ִϸ����� ������Ʈ
        owner.Anim.SetBool(owner.ParamID_IsMoving, false);
        
    }
    private IEnumerator CheckDestinationRoutine()
    {
        while ((owner.destination - owner.transform.position).sqrMagnitude > 0.5f)
        {
            yield return new WaitForSeconds(0.2f);
        }
        // �������� ������ ���
        Arrived();
    }
}
