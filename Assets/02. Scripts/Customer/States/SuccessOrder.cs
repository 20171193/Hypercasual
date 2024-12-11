using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuccessOrder : CustomerState
{
    public SuccessOrder(Customer owner)
    {
        this.owner = owner;
    }

    // 목적지 확인 코루틴
    private Coroutine checkDestinationRoutine;

    public override void Enter()
    {
        Debug.Log("Enter Success Order");
        owner.Agent.isStopped = false;
        owner.Agent.destination = owner.destination;
        checkDestinationRoutine = owner.StartCoroutine(CheckDestinationRoutine());
        // 애니메이터 업데이트
        owner.Anim.SetBool(owner.ParamID_IsMoving, true);
    }
    public override void Exit()
    {
        checkDestinationRoutine = null;
    }
    private void Arrived()
    {
        owner.Agent.isStopped = true;
        // 애니메이터 업데이트
        owner.Anim.SetBool(owner.ParamID_IsMoving, false);
        
    }
    private IEnumerator CheckDestinationRoutine()
    {
        while ((owner.destination - owner.transform.position).sqrMagnitude > 0.5f)
        {
            yield return new WaitForSeconds(0.2f);
        }
        // 목적지에 도달한 경우
        Arrived();
    }
}
