using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DineInTable : Table, IPlayerInteractable
{
    public enum State
    {
        Disable = 0,    // ��Ȱ��ȭ ���� (�ر� �ʿ�)
        Using,          // ���� ������� ����
        Used,           // ���� ����� �Ϸ��� ���� (������ ����)
        Enable          // Ȱ��ȭ ���� (�� ���밡��)
    }

    [Header("-Components")]
    [Tooltip("��/������ ���� ��ġ Ʈ������")]
    [SerializeField]
    private Transform spawnTr;
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
    private MoneySpawner moneySpawner;

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

    // ���̺��� �� ���, Ȱ��ȭ �� ��� ȣ��
    public UnityEvent OnTableEnable;

    private Coroutine payMoneyRoutine;

    private void Awake()
    {
        // �ر� ����Ҵ�
        RemainUnLockCost = unLockCost;
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
    //// �ֹ� ��⿭ ��ǥ ��ȯ (�� ��û : ī���� -> �Ĵ� ���̺�)
    //public Vector3 GetWaitingLine(Customer customer)
    //{

    //}
    //// �ٴ�� (ī���� -> ī����) 
    //public Vector3 GetWaitingLine(int order)
    //{

    //}
    // ������ ����
    public void SpawnTrash()
    {

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
            // �÷��̾� ��ġ�� ������ ��ġ �Ҵ�
            inst.transform.position = playerTransform.position; 
            // �� ����
            ScoreManager.Instance.PayMoney(1);
            // ���� �رݺ�� ó��
            RemainUnLockCost--;
            // ������ � ����
            StartCoroutine(Extension.BazierCurve(inst.transform, transform.position, payMoneySpeed));
            yield return new WaitForSeconds(payMoneyDelayTime);
        }
    }

    #endregion
}
