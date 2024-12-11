using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DineInTable : Table, IPlayerInteractable
{
    [Header("-Components")]
    [Tooltip("��/������ ���� ��ġ Ʈ������")]
    [SerializeField]
    private Transform spawnTr;
    [Tooltip("���̺� ��ġ Ʈ������")]
    [SerializeField]
    private Transform tableTr;
    public Transform TableTr {get { return tableTr; } }
    [Tooltip("��⿭ ��ġ Ʈ������")]
    [SerializeField]
    private Transform waitingLineTr;
    [Tooltip("�ر� ���� ������Ʈ ����")]
    [SerializeField]
    private GameObject unLockGroup;
    [Tooltip("�ر� ���� ������Ʈ ����")]
    [SerializeField]
    private GameObject lockGroup;
    [Tooltip("�ر� ��� �ؽ�Ʈ")]
    [SerializeField]
    private TextMeshProUGUI unLockCostText;
    [Tooltip("�� ������ ������")]
    [SerializeField]
    private Money moneyPrefab;
    [Tooltip("ũ�ξƻ� ������")]
    [SerializeField]
    private Croassant croassantPrefab;
    [Tooltip("������ ������Ʈ")]
    [SerializeField]
    private GameObject trashObject;

    [Space(10)]
    [Header("-Specs")]
    [Tooltip("�ر� ���")]
    [SerializeField]
    private int unLockCost;
    [Tooltip("�� ���� ������ �ð�")]
    [SerializeField]
    private float payMoneyDelayTime;
    [Tooltip("�� ���� �ӵ� (������)")]
    [SerializeField]
    private float payMoneySpeed;
    [Tooltip("��⿭ ����")]
    [SerializeField]
    private float waitingLineSpacing;

    [Space(10)]
    [Header("-Ballancing")]
    [Tooltip("���� ���� �رݺ��")]
    [SerializeField]
    private int remainUnLockCost;
    private int RemainUnLockCost 
    {
        set
        {
            remainUnLockCost = value;
            // UI ������Ʈ
            unLockCostText.text = remainUnLockCost.ToString();

            // �ر� ����
            if (remainUnLockCost == 0)
                UnLock();
        }
        get{ return remainUnLockCost; }
    }
    [Tooltip("�Ĵ� ���̺� �ر� ����")]
    [SerializeField]
    private bool isEnableTable = false;
    [Tooltip("�Ĵ��� ������� ���� �ִ��� Ȯ��")]
    [SerializeField]
    private bool isInCustomer = false;
    [Tooltip("�÷��̾�� ��ȣ�ۿ� ������ Ȯ��")]
    [SerializeField]
    private bool isInPlayer = false;

    // �ֹ���û ť
    private Queue<Customer> watingCustomerQueue;

    [Space(10)]
    [Header("-Events")]
    // ���̺��� �� ���, Ȱ��ȭ �� ��� ȣ��
    public UnityEvent OnTableEnable;

    private Coroutine payMoneyRoutine;
    private Coroutine trashReleaseDelay;
    private void Awake()
    {
        // �ر� ����Ҵ�
        RemainUnLockCost = unLockCost;

        watingCustomerQueue = new Queue<Customer>();
    }

    // ���̺� �ر�
    private void UnLock()
    {
        isEnableTable = true;

        // �رݿ� ���� ������Ʈ Ȱ/��Ȱ��ȭ
        unLockGroup.SetActive(true);
        lockGroup.SetActive(false);

        OnTableEnable?.Invoke();
    }

    #region �� ��ȣ�ۿ�
    // �ֹ� ��⿭ ��ǥ ��ȯ (�� ��û : ī���� -> �Ĵ� ���̺�)
    public Vector3 GetWaitingLine(Customer customer)
    {
        // ���̺��� Ȱ��ȭ �Ǿ����� ���� ���
        if(isEnableTable == false || (isEnableTable == true && isInCustomer))
        { 
            int count = watingCustomerQueue.Count;
            watingCustomerQueue.Enqueue(customer);
            customer.orderTurn = count;
            return waitingLineTr.position + (waitingLineTr.forward * waitingLineSpacing * count);
        }
        // ���̺��� Ȱ��ȭ �Ǿ��ִ� ���
        else
        {
            customer.orderTurn = -1;
            isInCustomer = true;
            return tableTr.position;
        }
    }
    // �ٴ�� (ī���� -> ī����, ī���� -> ���̺�) 
    public Vector3 GetWaitingLine(int order)
    {
        // ���̺� �̵�
        if(order < 0)
        {
            isInCustomer = true;
            return tableTr.position;
        }
        // ��⿭ �̵�
        else
        {
            return waitingLineTr.position + (waitingLineTr.forward * waitingLineSpacing * order);
        }
    }
    // ������ ����
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

    #region �÷��̾� ��ȣ�ۿ�
    public void EnterPlayer(PlayerItemController playerItemController)
    {
        isInPlayer = true;

        // ���̺��� ��Ȱ��ȭ ������ ���
        if(!isEnableTable)
        {
            if(ScoreManager.Instance.CurMoney > 0)
            {
                // ���� ����
                payMoneyRoutine = StartCoroutine(PayMoneyRoutine(playerItemController.transform));
            }
        }
        // ���̺��� Ȱ��ȭ ������ ���
        else
        {
            // ������ Ȯ�� �� ����
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
            // �� ����
            ScoreManager.Instance.PayMoney(1);
            // ���� �رݺ�� ó��
            RemainUnLockCost -= 1;
            // ������ ����
            Money inst = PoolManager.Instance.GetPool(moneyPrefab, playerTransform.position, playerTransform.rotation) as Money;
            // ������ � ����
            StartCoroutine(MoneyBazierCurve(inst));
            yield return new WaitForSeconds(payMoneyDelayTime);
        }
    }
    private IEnumerator MoneyBazierCurve(Money targetMoney)
    {
        // ���� �Ϸ� �� �� ������ �ν��Ͻ� ��Ȱ��ȭ
        yield return Extension.BazierCurve(targetMoney.transform, transform.position, payMoneySpeed);
        targetMoney.Release();
    }

    // ������ ���� ������
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
