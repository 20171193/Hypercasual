using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DineInTable : Table, IPlayerInteractable
{
    [Header("-Components")]
    [Tooltip("빵/쓰레기 스폰 위치 트랜스폼")]
    [SerializeField]
    private Transform spawnTr;
    [Tooltip("테이블 위치 트랜스폼")]
    [SerializeField]
    private Transform tableTr;
    public Transform TableTr {get { return tableTr; } }
    [Tooltip("대기열 위치 트랜스폼")]
    [SerializeField]
    private Transform waitingLineTr;
    [Tooltip("해금 이전 오브젝트 모음")]
    [SerializeField]
    private GameObject unLockGroup;
    [Tooltip("해금 이후 오브젝트 모음")]
    [SerializeField]
    private GameObject lockGroup;
    [Tooltip("해금 비용 텍스트")]
    [SerializeField]
    private TextMeshProUGUI unLockCostText;
    [Tooltip("돈 아이템 프리팹")]
    [SerializeField]
    private Money moneyPrefab;
    [Tooltip("크로아상 프리팹")]
    [SerializeField]
    private Croassant croassantPrefab;
    [Tooltip("쓰레기 오브젝트")]
    [SerializeField]
    private GameObject trashObject;

    [Space(10)]
    [Header("-Specs")]
    [Tooltip("해금 비용")]
    [SerializeField]
    private int unLockCost;
    [Tooltip("돈 지불 딜레이 시간")]
    [SerializeField]
    private float payMoneyDelayTime;
    [Tooltip("돈 지불 속도 (베지어곡선)")]
    [SerializeField]
    private float payMoneySpeed;
    [Tooltip("대기열 간격")]
    [SerializeField]
    private float waitingLineSpacing;

    [Space(10)]
    [Header("-Ballancing")]
    [Tooltip("현재 남은 해금비용")]
    [SerializeField]
    private int remainUnLockCost;
    private int RemainUnLockCost 
    {
        set
        {
            remainUnLockCost = value;
            // UI 업데이트
            unLockCostText.text = remainUnLockCost.ToString();

            // 해금 진행
            if (remainUnLockCost == 0)
                UnLock();
        }
        get{ return remainUnLockCost; }
    }
    [Tooltip("식당 테이블 해금 상태")]
    [SerializeField]
    private bool isEnableTable = false;
    [Tooltip("식당을 사용중인 고객이 있는지 확인")]
    [SerializeField]
    private bool isInCustomer = false;
    [Tooltip("플레이어와 상호작용 중인지 확인")]
    [SerializeField]
    private bool isInPlayer = false;

    // 주문요청 큐
    private Queue<Customer> watingCustomerQueue;

    [Space(10)]
    [Header("-Events")]
    // 테이블이 빈 경우, 활성화 된 경우 호출
    public UnityEvent OnTableEnable;

    private Coroutine payMoneyRoutine;
    private Coroutine trashReleaseDelay;
    private void Awake()
    {
        // 해금 비용할당
        RemainUnLockCost = unLockCost;

        watingCustomerQueue = new Queue<Customer>();
    }

    // 테이블 해금
    private void UnLock()
    {
        isEnableTable = true;

        // 해금에 따른 오브젝트 활/비활성화
        unLockGroup.SetActive(true);
        lockGroup.SetActive(false);

        OnTableEnable?.Invoke();
    }

    #region 고객 상호작용
    // 주문 대기열 좌표 반환 (고객 요청 : 카운터 -> 식당 테이블)
    public Vector3 GetWaitingLine(Customer customer)
    {
        // 테이블이 활성화 되어있지 않은 경우
        if(isEnableTable == false || (isEnableTable == true && isInCustomer))
        { 
            int count = watingCustomerQueue.Count;
            watingCustomerQueue.Enqueue(customer);
            customer.orderTurn = count;
            return waitingLineTr.position + (waitingLineTr.forward * waitingLineSpacing * count);
        }
        // 테이블이 활성화 되어있는 경우
        else
        {
            customer.orderTurn = -1;
            isInCustomer = true;
            return tableTr.position;
        }
    }
    // 줄당김 (카운터 -> 카운터, 카운터 -> 테이블) 
    public Vector3 GetWaitingLine(int order)
    {
        // 테이블 이동
        if(order < 0)
        {
            isInCustomer = true;
            return tableTr.position;
        }
        // 대기열 이동
        else
        {
            return waitingLineTr.position + (waitingLineTr.forward * waitingLineSpacing * order);
        }
    }
    // 쓰레기 스폰
    public void SpawnTrash()
    {
        trashObject.SetActive(true);

        if(isInPlayer && trashReleaseDelay == null)
        {
            trashReleaseDelay = StartCoroutine(ReleaseTrashDelay());
        }
    }
    public Croassant SpawnCroassant()
    {
        Croassant inst = PoolManager.Instance.GetPool(croassantPrefab, spawnTr.position, spawnTr.rotation) as Croassant;
        return inst;
    }
    #endregion

    #region 플레이어 상호작용
    public void EnterPlayer(PlayerItemController playerItemController)
    {
        isInPlayer = true;

        // 테이블이 비활성화 상태일 경우
        if(!isEnableTable)
        {
            if(ScoreManager.Instance.CurMoney > 0)
            {
                // 구매 실행
                payMoneyRoutine = StartCoroutine(PayMoneyRoutine(playerItemController.transform));
            }
        }
        // 테이블이 활성화 상태일 경우
        else
        {
            // 쓰레기 확인 후 수거
            if(trashObject.activeSelf == true && trashReleaseDelay == null)
            {
                trashReleaseDelay = StartCoroutine(ReleaseTrashDelay());
            }
        }
    }
    public void ExitPlayer()
    {
        isInPlayer = false;
    }
    private IEnumerator PayMoneyRoutine(Transform playerTransform)
    {
        while(ScoreManager.Instance.CurMoney > 0 && RemainUnLockCost > 0)
        {
            // 돈 지불
            ScoreManager.Instance.PayMoney(1);
            // 남은 해금비용 처리
            RemainUnLockCost -= 1;
            // 아이템 스폰
            Money inst = PoolManager.Instance.GetPool(moneyPrefab, playerTransform.position, playerTransform.rotation) as Money;
            // 베지어 곡선 적용
            StartCoroutine(MoneyBazierCurve(inst));
            yield return new WaitForSeconds(payMoneyDelayTime);
        }
    }
    private IEnumerator MoneyBazierCurve(Money targetMoney)
    {
        // 도착 완료 후 돈 아이템 인스턴스 비활성화
        yield return Extension.BazierCurve(targetMoney.transform, transform.position, payMoneySpeed);
        targetMoney.Release();
    }

    // 쓰레기 수거 딜레이
    private IEnumerator ReleaseTrashDelay()
    {
        watingCustomerQueue.Dequeue();
        yield return new WaitForSeconds(1.5f);
        OnTableEnable?.Invoke();
        trashObject.SetActive(false);

        isInCustomer = false;
        trashReleaseDelay = null;
    }
    #endregion
}
