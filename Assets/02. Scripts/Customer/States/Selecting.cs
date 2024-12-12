using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 빵 보관소로 이동 및 주문 대기상태
/// </summary>
public class Selecting : CustomerState
{
    public Selecting(Customer owner)
    {
        this.owner = owner;
    }
    // 목적지 확인 코루틴
    private Coroutine checkDestinationRoutine;

    // 경유지 할당
    private Vector3 transitPos;

    public override void Enter()
    {
        transitPos = OrderManager.Instance.basketTransitTr.position;

        owner.Agent.isStopped = false;
        owner.Agent.destination = transitPos;
        checkDestinationRoutine = owner.StartCoroutine(CheckDestinationRoutine());
        // 애니메이터 업데이트
        owner.Anim.SetBool(owner.ParamID_IsMoving, true);
    }
    public override void Exit()
    {
        checkDestinationRoutine = null;
    }

    // 빵 보관소에 도달한 경우
    private void Arrived()
    {
        // 애니메이터 업데이트 (걷기 비활성화)
        owner.Anim.SetBool(owner.ParamID_IsMoving, false);
        owner.Agent.isStopped = true;
        owner.Agent.transform.position = owner.destination;

        // 빵 보관소 방향으로 회전
        owner.transform.forward = (OrderManager.Instance.basket.transform.position - owner.transform.position).normalized;

        // 빵 주문요청
        OrderManager.Instance.basket.RequestOrder(owner.OwnRequest);
    }

    // 목적지 확인 루틴
    private IEnumerator CheckDestinationRoutine()
    {
        // 경유지까지 도착 확인
        while((transitPos - owner.transform.position).sqrMagnitude > 0.5f)
        {
            yield return new WaitForSeconds(0.2f);
        }

        owner.Agent.destination = owner.destination;

        // 목적지까지 도착 확인
        while ((owner.destination - owner.transform.position).sqrMagnitude > 0.5f)
        {
            yield return new WaitForSeconds(0.2f);
        }

        // 목적지에 도달한 경우
        Arrived();
    }
}
