using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Counter : Table, IPlayerInteractable
{
    [Header("-Components")]
    [Tooltip("포장용지 스포너")]
    [SerializeField]
    private PaperBagSpawner paperBagSpawner;
    [Tooltip("포장속도 적용")]
    [SerializeField]
    private float packagingSpeed;
    [Tooltip("돈 아이템 스택")]
    [SerializeField]
    private MoneyStack moneyStack;

    [Space(10)]
    [Header("-Specs")]
    [Tooltip("대기열 기준 트랜스폼 (해당 트랜스폼 기준 z열)")]
    [SerializeField]
    private Transform waitingLineTr;
    [Tooltip("대기열 줄 간격")]
    [SerializeField]
    private float waitingLineSpacing;

    private Queue<Customer> waitingCustomerQueue;

    // 1순위 고객 할당
    private Customer firstOrderCustomer = null;

    // 주문처리 완료 후 Invoke
    public UnityEvent OnProcessedOrder;

    [Space(10)]
    [Header("-Ballancing")]
    [Tooltip("플레이어가 대기중인지 확인")]
    [SerializeField]
    private bool isInPlayer = false;

    private Coroutine packagingRoutine;

    private void Awake()
    {
        waitingCustomerQueue = new Queue<Customer>();
    }

    #region 플레이어-고객 상호작용
    // 주문 처리
    public void ProcessOrder()
    {
        // 플레이어나 고객이 존재하지 않을 경우
        if (!isInPlayer || firstOrderCustomer == null)
            return;

        if (packagingRoutine != null)
            StopCoroutine(packagingRoutine);

        packagingRoutine = StartCoroutine(PackagingDelay());
    }
    #endregion

    #region 고객 상호작용

    // 주문 요청
    public void RequestOrder(Customer customer)
    {
        // 우선순위에 맞지 않는 요청은 거부
        if (waitingCustomerQueue.Peek() != customer)
            return;

        firstOrderCustomer = customer;

        // 플레이어가 존재할 경우 주문 처리
        if (isInPlayer)
            ProcessOrder();
    }
    // 주문 대기열 좌표 반환 (고객 요청 : 빵 저장소 -> 카운터)
    public Vector3 GetWatingLine(Customer customer)
    {
        int count = waitingCustomerQueue.Count;
        waitingCustomerQueue.Enqueue(customer);
        // 주문처리 순서할당
        customer.orderTurn = count; 
        return waitingLineTr.position + (waitingLineTr.forward * waitingLineSpacing * count);
    }
    // 줄당김 (카운터 -> 카운터) 
    public Vector3 GetWatingLine(int order)
    {
        return waitingLineTr.position + (waitingLineTr.forward * waitingLineSpacing * (order));
    }   
    // 돈 지불
    public void PayMoney(int count)
    {
        moneyStack.SpawnMoney(count);
    }
    #endregion

    #region 플레이어 상호작용
    public void EnterPlayer(PlayerItemController playerItemController)
    {
        isInPlayer = true;
       
        // 현재 대기중인 고객이 없을 경우
        if (waitingCustomerQueue.Count == 0 || firstOrderCustomer == null)
            return;

        // 주문 처리
        ProcessOrder();
    }
    public void ExitPlayer()
    {
        isInPlayer = false;
    }
    #endregion

    // 아이템 포장 딜레이
    private IEnumerator PackagingDelay()
    {
        // 포장용지 스폰
        PaperBag paperBag = paperBagSpawner.Spawn() as PaperBag;
        ItemStack targetStack = firstOrderCustomer.ItemController.ItemStack;
        waitingCustomerQueue.Dequeue();

        while (targetStack.CurStackCount > 0)
        {
            Item getItem = targetStack.PopItem();
            // 포물선 효과 적용
            yield return BazierCurve(getItem.transform, transform.position);
            // 아이템 비활성화
            getItem.Release();
        }

        // 주문완료 처리
        // 포장용지 전달
        firstOrderCustomer.SendItem(paperBag);
        // 초기화
        firstOrderCustomer = null;
        // 고객이 떠날때까지 딜레이
        yield return new WaitForSeconds(1f);
        OnProcessedOrder?.Invoke();
    }
    private IEnumerator BazierCurve(Transform targetTransform, Vector3 destination)
    {
        Vector3 startPos = targetTransform.position;
        Vector3 midPos = Vector3.Lerp(startPos, destination, 0.5f) + Vector3.up * 5f;
        float rate = 0f;
        while(rate < 1f)
        {
            Vector3 p1 = Vector3.Lerp(startPos, midPos, rate);
            Vector3 p2 = Vector3.Lerp(midPos, destination, rate);
            targetTransform.position = Vector3.Lerp(p1, p2, rate);
            rate += Time.deltaTime * packagingSpeed; // 포장 속도 적용
            yield return null;
        }
        targetTransform.position = destination;
    }
}
