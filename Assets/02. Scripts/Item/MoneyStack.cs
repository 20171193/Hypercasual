using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 돈 아이템 스택
/// </summary>
public class MoneyStack : MonoBehaviour, IPlayerInteractable
{
    [Header("-Components")]
    [Tooltip("돈 아이템 스포너")]
    [SerializeField]
    private MoneySpawner moneySpanwer;

    [Tooltip("돈 아이템 프리팹")]
    [SerializeField]
    private Money moneyPrefab;

    [Space(10)]
    [Header("-Specs")]
    [Tooltip("최대 행")]
    [SerializeField]
    private int MaxRow;
    [Tooltip("최대 열")]
    [SerializeField]
    private int MaxCol;
    [Tooltip("패딩 크기")]
    [SerializeField]
    private Vector3 paddingSize;
    [Tooltip("아이템 습득 시간")]
    [SerializeField]
    private float itemGetTime;
    [Tooltip("아이템 습득 속도")]
    [SerializeField]
    private float itemGetSpeed;

    private Stack<Money> moneyStack;

    [Space(10)]
    [Header("-Ballancing")]
    [Tooltip("대기열에 플레이어가 있는지 확인")]
    [SerializeField]
    private bool isInPlayer = false;
    private PlayerItemController playerItemController = null;

    // 돈 아이템 크기
    private Vector3 itemSize;
    // 아이템 스택 간격
    private Vector3 itemSpacing;

    private Coroutine moneyPopRoutine;

    private void Awake()
    {
        moneyStack = new Stack<Money>();
        // 아이템 크기 할당
        itemSize = moneyPrefab.ItemRenderer.bounds.size;
        // 아이템 간격 할당
        itemSpacing = new Vector3(itemSize.x / 2f + paddingSize.x
            , itemSize.y / 2f + paddingSize.y
            , itemSize.z / 2f + paddingSize.z);
    }

    #region 고객 상호작용
    public void SpawnMoney(int spawnCount)
    {
        int xAxis = -1;
        int yAxis = -1;
        int zAxis = -1;
        int curSpawnCount = moneyStack.Count;
        // spawnCount만큼 아이템 스폰
        for (int i =0; i<spawnCount; i++)
        {
            Money moneyItem = moneySpanwer.Spawn() as Money;
            if(moneyItem == null)
            {
                Debug.Log("Money 아이템 스폰 실패");
                return;
            }

            curSpawnCount += 1;
            // 스폰될 위치 설정
            xAxis = (curSpawnCount-1) % MaxRow;             // 열
            zAxis = ((curSpawnCount - 1)/MaxRow) % MaxCol;  // 행 
            yAxis = (curSpawnCount-1) / (MaxRow * MaxCol);      // 높이

            moneyItem.transform.parent = transform;
            moneyItem.transform.localPosition = new Vector3(xAxis * itemSpacing.x
                , yAxis * itemSpacing.y
                , zAxis * itemSpacing.z);

            moneyStack.Push(moneyItem);
        }

        // 대기열에 플레이어가 있다면 아이템 전달
        if(isInPlayer && playerItemController != null)
        {
            if (moneyPopRoutine != null)
                StopCoroutine(moneyPopRoutine);

            moneyPopRoutine = StartCoroutine(MoneyPopRoutine(playerItemController));
        }
    }
    #endregion

    #region 플레이어 상호작용
    public void EnterPlayer(PlayerItemController playerItemController)
    {
        // 플레이어 상호작용 체크
        isInPlayer = true;
        this.playerItemController = playerItemController;

        if (moneyStack.Count == 0)
            return;

        moneyPopRoutine = StartCoroutine(MoneyPopRoutine(playerItemController));
    }
    public void ExitPlayer()
    {
        // 플레이어 상호작용 해제
        isInPlayer = false;
        playerItemController = null;
    }
    #endregion

    private IEnumerator MoneyPopRoutine(PlayerItemController playerItemController)
    {
        while(moneyStack.Count > 0)
        {
            Money money = moneyStack.Pop();
            // 돈 아이템 습득
            ScoreManager.Instance.GetMoney(1);
            // 베지어 곡선 적용
            StartCoroutine(MoneyBazierCurve(money, playerItemController.transform.position));
            yield return new WaitForSeconds(itemGetTime);
        }
    }
    private IEnumerator MoneyBazierCurve(Money targetMoney, Vector3 destination)
    {
        // 도착 완료 후 돈 아이템 인스턴스 비활성화
        yield return Extension.BazierCurve(targetMoney.transform, transform.position, itemGetSpeed);
        targetMoney.Release();
    }
}
