using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DineInTable : Table, IPlayerInteractable
{
    public enum State
    {
        Disable = 0,    // 비활성화 상태 (해금 필요)
        Using,          // 고객이 사용중인 상태
        Used,           // 고객이 사용을 완료한 상태 (쓰레기 존재)
        Enable          // 활성화 상태 (고객 수용가능)
    }

    [Header("-Components")]
    [Tooltip("빵/쓰레기 스폰 위치 트랜스폼")]
    [SerializeField]
    private Transform spawnTr;
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
    [Tooltip("돈 아이템 스포너")]
    [SerializeField]
    private MoneySpawner moneySpawner;

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

    // 테이블이 빈 경우, 활성화 된 경우 호출
    public UnityEvent OnTableEnable;

    private Coroutine payMoneyRoutine;

    private void Awake()
    {
        // 해금 비용할당
        RemainUnLockCost = unLockCost;
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
    //// 주문 대기열 좌표 반환 (고객 요청 : 카운터 -> 식당 테이블)
    //public Vector3 GetWaitingLine(Customer customer)
    //{

    //}
    //// 줄당김 (카운터 -> 카운터) 
    //public Vector3 GetWaitingLine(int order)
    //{

    //}
    // 쓰레기 스폰
    public void SpawnTrash()
    {

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

        }
    }
    public void ExitPlayer()
    {
        isInPlayer = false;
    }

    private IEnumerator PayMoneyRoutine(Transform playerTransform)
    {
        while(ScoreManager.Instance.CurMoney > 0)
        {
            Money inst = moneySpawner.Spawn() as Money;
            // 플레이어 위치에 아이템 위치 할당
            inst.transform.position = playerTransform.position; 
            // 돈 지불
            ScoreManager.Instance.PayMoney(1);
            // 남은 해금비용 처리
            RemainUnLockCost--;
            // 베지어 곡선 적용
            StartCoroutine(Extension.BazierCurve(inst.transform, transform.position, payMoneySpeed));
            yield return new WaitForSeconds(payMoneyDelayTime);
        }
    }

    #endregion
}
