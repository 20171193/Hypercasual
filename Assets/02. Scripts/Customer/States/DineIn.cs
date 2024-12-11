using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DineIn : CustomerState
{
    public DineIn(Customer owner)
    {
        this.owner = owner;
    }

    // 목적지 확인 코루틴
    private Coroutine checkDestinationRoutine;

    // 식사 딜레이
    private Coroutine eatingDelay;

    public override void Enter()
    {
        owner.Agent.isStopped = false;
        owner.Agent.destination = owner.destination;
        checkDestinationRoutine = owner.StartCoroutine(CheckDestinationRoutine());
        // 애니메이터 업데이트
        owner.Anim.SetBool(owner.ParamID_IsMoving, true);

        // 대기열에 등록된 경우 이벤트 등록 (줄당김)
        if (owner.orderTurn >= 0)
            OrderManager.Instance.dineIn.OnTableEnable.AddListener(ShiftLine);
    }

    public override void Exit()
    {
        // 이벤트 해제 (줄당김)
        OrderManager.Instance.dineIn.OnTableEnable.RemoveListener(ShiftLine);

        checkDestinationRoutine = null;
    }

    // 줄 당기기
    private void ShiftLine()
    {
        // 목적지 재설정
        owner.destination = OrderManager.Instance.dineIn.GetWaitingLine(--owner.orderTurn);

        // 이동
        owner.Agent.isStopped = false;
        owner.Agent.destination = owner.destination;
        // 애니메이터 업데이트
        owner.Anim.SetBool(owner.ParamID_IsMoving, true);

        checkDestinationRoutine = owner.StartCoroutine(CheckDestinationRoutine());
    }

    private void Arrived()
    {
        owner.Agent.isStopped = true;
        // 애니메이터 업데이트
        owner.Anim.SetBool(owner.ParamID_IsMoving, false);
        // 패키징할 순서일 경우 패키징 요청
        if (owner.orderTurn < 0)
        {
            Debug.Log("패키징 요청");
            // 방향 할당 (테이블 방향)
            owner.transform.forward = OrderManager.Instance.dineIn.TableTr.forward;
            eatingDelay = owner.StartCoroutine(EatingDelay());
        }
        else
        {
            // 방향 할당 (카운터 방향)
            owner.transform.forward = (OrderManager.Instance.counter.transform.position - owner.transform.position).normalized;
        }
    }

    private IEnumerator CheckDestinationRoutine()
    {
        // 목적지까지 도착 확인
        while ((owner.destination - owner.transform.position).sqrMagnitude > 0.5f)
        {
            yield return new WaitForSeconds(0.2f);
        }

        // 목적지에 도달한 경우
        Arrived();
    }

    private IEnumerator EatingDelay()
    {
        // 소유한 빵 제거
        owner.ItemController.ItemStack.PopItem().Release();
        // 빵 스폰
        Croassant croassant = OrderManager.Instance.dineIn.SpawnCroassant();
        yield return new WaitForSeconds(3f);
        croassant.Release();
        // 쓰레기 스폰
        OrderManager.Instance.dineIn.SpawnTrash();

        // 목적지 설정
        owner.destination = OrderManager.Instance.entranceTr.position;
        // 상태 전이
        owner.FSM.ChangeState("SuccessOrder");
    }
}
