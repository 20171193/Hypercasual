using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;

// 빵 저장소의 대기열이 비어있을 경우 고객 스폰 요청

/// <summary>
/// 빵 저장소 테이블, 고객 스폰 및 주문 할당
/// </summary>
public class Basket : Table, IPlayerInteractable
{
    [Header("-Components")]
    [SerializeField]
    private ItemStack basketStack;

    [SerializeField]
    private CustomerSpawner customerSpawner;

    [Header("-Specs")]
    [Tooltip("주문 대기열 위치")]
    [SerializeField]
    private Transform[] waitingLineTransform;

    [Header("-Ballancing")]
    [Tooltip("주문요청 큐")]
    [SerializeField]
    private Queue<RequestCroassant> requests;
    [Tooltip("현재 진행중인 요청")]
    [SerializeField]
    private RequestCroassant? curRequest = null;
    [Tooltip("대기열 내 고객 (주문 대기열과 1:1)")]
    [SerializeField]
    private Customer[] inCustomers;
    
    [Tooltip("플레이어 아이템 컨트롤러 (트리거 시 할당)")]
    [SerializeField]
    private PlayerItemController playerItemController = null;
    [Tooltip("플레이어가 대기중인지 확인")]
    [SerializeField]
    private bool isInPlayer = false;

    private Coroutine croassantPushRoutine;
    private Coroutine croassantPopRoutine;
    private Coroutine spawnCustomerRoutine;

    private void Awake()
    {
        requests = new Queue<RequestCroassant>();

        // 대기열 개수 만큼 배열할당 
        inCustomers = new Customer[waitingLineTransform.Length];

        customerSpawner.OnInitSpanwer.AddListener(SpawnCustomer);
    }
    private void OnEnable()
    {
        // 주문처리 콜백 등록
        basketStack.OnStackedItem.AddListener(ProcessOrder);
    }
    private void OnDisable()
    {
        basketStack.OnStackedItem.RemoveListener(ProcessOrder);
    }

    // 고객스폰 요청
    private void SpawnCustomer()
    {
        if(spawnCustomerRoutine != null)
            StopCoroutine(spawnCustomerRoutine);

        spawnCustomerRoutine = StartCoroutine(SpawnCustomerRoutine());
    }

    // 빈 대기열 찾기
    private bool GetWaitingLine(out Vector3 waitingLinePosition, out int waitingLineIndex)
    {
        waitingLinePosition = Vector3.zero;
        waitingLineIndex = -1;
        for (int i = 0; i < inCustomers.Length; i++)
        {
            if (inCustomers[i] == null)
            {
                // 대기열 위치 할당 (out 반환)
                waitingLinePosition = waitingLineTransform[i].position;
                // 대기열 인덱스 할당
                waitingLineIndex = i;
                return true;
            }
        }
        return false;
    }

    private IEnumerator SpawnCustomerRoutine()
    {
        // 대기열이 가득 찰때까지 스폰
        while (GetWaitingLine(out Vector3 waitingLinePosition, out int watingLineIndex))
        {
            Customer customer = customerSpawner.Spawn() as Customer;
            // 대기열에 고객 할당
            inCustomers[watingLineIndex] = customer;
            // 목적지 할당
            customer.destination = waitingLinePosition;
            // 대기열 인덱스 할당
            customer.orderTurn = watingLineIndex;
            // 빵 선택 상태로 변경
            customer.FSM.ChangeState("Selecting");
            yield return new WaitForSeconds(1f);
        }
        spawnCustomerRoutine = null;
    }

    #region 고객 상호작용
    // 주문 처리 (OnStackedItem Invoke 시 발생)
    private void ProcessOrder()
    {
        // 주문 요청이 없거나 스택에 빵이 존재하지 않을 경우 return
        if (requests.Count == 0 || basketStack.CurStackCount == 0)
            return;

        // 플레이어가 대기중일 경우 빵 저장
        if (isInPlayer && playerItemController != null)
            croassantPushRoutine = StartCoroutine(CroassantPushRoutine(playerItemController.ItemStack));

        // 주문 진행
        if (croassantPopRoutine == null)
            croassantPopRoutine = StartCoroutine(CroassantPopRoutine());
    }
    // 주문 요청 
    public void RequestOrder(Customer customer, int count)
    {
        // 새 요청 생성
        RequestCroassant request = new RequestCroassant(customer, count);
        // 주문 대기열 큐 할당
        requests.Enqueue(request);
        // 주문 처리
        ProcessOrder();
    }
    // 빵 전달 딜레이 루틴 
    private IEnumerator CroassantPopRoutine()
    {
        while(requests.Count > 0)
        {
            // 진행할 요청 할당
            curRequest = requests.Dequeue();
            Customer customer = curRequest.Value.customer;
            int count = curRequest.Value.count;
            // 0.1초 딜레이 이후 전달
            while(basketStack.CurStackCount > 0 && count-- > 0)
            {
                // 주문요청이 모두 처리된 경우
                if (count == 0)
                {
                    inCustomers[customer.orderTurn] = null;
                    // 새 고객 생성
                    SpawnCustomer();
                }

                customer.SendItem(basketStack.PopItem());
                yield return new WaitForSeconds(0.1f);
            }
        }

        croassantPopRoutine = null;
    }
    #endregion

    #region 플레이어 상호작용
    // 빵 보관 
    public void EnterPlayer(PlayerItemController targetController)
    {
        if (targetController.ItemStack.CurStackCount == 0)
            return;

        isInPlayer = true;
        // 현재 스택이 가득찬 상태
        if (basketStack.isFull)
        {
            // 플레이어 컨트롤러 대기열에 할당
            playerItemController = targetController;
            return;
        }

        if (croassantPushRoutine != null)
            StopCoroutine(croassantPushRoutine);

        croassantPushRoutine = StartCoroutine(CroassantPushRoutine(targetController.ItemStack));
    }
    public void ExitPlayer()
    {
        isInPlayer = false;
    }
    // 빵 저장 딜레이 루틴 
    private IEnumerator CroassantPushRoutine(ItemStack targetStack)
    {
        // 플레이어 스택 대기열 할당해제
        while (targetStack.CurStackCount > 0 && !basketStack.isFull)
        {
            // 플레이어가 탈출한 경우 루틴 탈출
            if (!isInPlayer)
            {
                croassantPushRoutine = null;
                yield break;
            }

            // 빵 저장소 스택으로 이동
            basketStack.PushItem(targetStack.PopItem());
            yield return new WaitForSeconds(0.1f);
        }

        croassantPushRoutine = null;
    }
    #endregion
}
