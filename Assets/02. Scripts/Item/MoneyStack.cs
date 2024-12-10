using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �� ������ ����
/// </summary>
public class MoneyStack : MonoBehaviour, IPlayerInteractable
{
    [Header("-Components")]
    [Tooltip("�� ������ ������")]
    [SerializeField]
    private MoneySpawner moneySpanwer;

    [Tooltip("�� ������ ������")]
    [SerializeField]
    private Money moneyPrefab;

    [Space(10)]
    [Header("-Specs")]
    [Tooltip("�ִ� ��")]
    [SerializeField]
    private int MaxRow;
    [Tooltip("�ִ� ��")]
    [SerializeField]
    private int MaxCol;
    [Tooltip("�е� ũ��")]
    [SerializeField]
    private Vector3 paddingSize;
    [Tooltip("������ ���� �ð�")]
    [SerializeField]
    private float itemGetTime;
    [Tooltip("������ ���� �ӵ�")]
    [SerializeField]
    private float itemGetSpeed;

    private Stack<Money> moneyStack;

    [Space(10)]
    [Header("-Ballancing")]
    [Tooltip("��⿭�� �÷��̾ �ִ��� Ȯ��")]
    [SerializeField]
    private bool isInPlayer = false;
    private PlayerItemController playerItemController = null;

    // �� ������ ũ��
    private Vector3 itemSize;
    // ������ ���� ����
    private Vector3 itemSpacing;

    private Coroutine moneyPopRoutine;

    private void Awake()
    {
        moneyStack = new Stack<Money>();
        // ������ ũ�� �Ҵ�
        itemSize = moneyPrefab.ItemRenderer.bounds.size;
        // ������ ���� �Ҵ�
        itemSpacing = new Vector3(itemSize.x / 2f + paddingSize.x
            , itemSize.y / 2f + paddingSize.y
            , itemSize.z / 2f + paddingSize.z);
    }

    #region �� ��ȣ�ۿ�
    public void SpawnMoney(int spawnCount)
    {
        int xAxis = -1;
        int yAxis = -1;
        int zAxis = -1;
        int curSpawnCount = moneyStack.Count;
        // spawnCount��ŭ ������ ����
        for (int i =0; i<spawnCount; i++)
        {
            Money moneyItem = moneySpanwer.Spawn() as Money;
            if(moneyItem == null)
            {
                Debug.Log("Money ������ ���� ����");
                return;
            }

            curSpawnCount += 1;
            // ������ ��ġ ����
            xAxis = (curSpawnCount-1) % MaxRow;             // ��
            zAxis = ((curSpawnCount - 1)/MaxRow) % MaxCol;  // �� 
            yAxis = (curSpawnCount-1) / (MaxRow * MaxCol);      // ����

            moneyItem.transform.parent = transform;
            moneyItem.transform.localPosition = new Vector3(xAxis * itemSpacing.x
                , yAxis * itemSpacing.y
                , zAxis * itemSpacing.z);

            moneyStack.Push(moneyItem);
        }

        // ��⿭�� �÷��̾ �ִٸ� ������ ����
        if(isInPlayer && playerItemController != null)
        {
            if (moneyPopRoutine != null)
                StopCoroutine(moneyPopRoutine);

            moneyPopRoutine = StartCoroutine(MoneyPopRoutine(playerItemController));
        }
    }
    #endregion

    #region �÷��̾� ��ȣ�ۿ�
    public void EnterPlayer(PlayerItemController playerItemController)
    {
        // �÷��̾� ��ȣ�ۿ� üũ
        isInPlayer = true;
        this.playerItemController = playerItemController;

        if (moneyStack.Count == 0)
            return;

        moneyPopRoutine = StartCoroutine(MoneyPopRoutine(playerItemController));
    }
    public void ExitPlayer()
    {
        // �÷��̾� ��ȣ�ۿ� ����
        isInPlayer = false;
        playerItemController = null;
    }
    #endregion

    private IEnumerator MoneyPopRoutine(PlayerItemController playerItemController)
    {
        while(moneyStack.Count > 0)
        {
            Money money = moneyStack.Pop();
            // ������ � ����
            StartCoroutine(Extension.BazierCurve(money.transform, playerItemController.transform.position, itemGetSpeed));
            // �� ������ ����
            ScoreManager.Instance.GetMoney(1);
            // �� ������ ��Ȱ��ȭ
            money.Release();
            yield return new WaitForSeconds(itemGetTime);
        }
    }

    private IEnumerator BazierCurve(Transform targetTransform, Vector3 destination)
    {
        Vector3 startPos = targetTransform.position;
        Vector3 midPos = Vector3.Lerp(startPos, destination, 0.5f) + Vector3.up * 5f;
        float rate = 0f;
        while (rate < 1f)
        {
            Vector3 p1 = Vector3.Lerp(startPos, midPos, rate);
            Vector3 p2 = Vector3.Lerp(midPos, destination, rate);
            targetTransform.position = Vector3.Lerp(p1, p2, rate);
            rate += Time.deltaTime * itemGetSpeed; // ���� �ӵ� ����
            yield return null;
        }
        targetTransform.position = destination;
    }
}
