using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Counter : Table, IPlayerInteractable
{
    [Header("-Components")]
    [Tooltip("������� ������")]
    [SerializeField]
    private PaperBagSpawner paperBagSpawner;
    [Tooltip("����ӵ� ����")]
    [SerializeField]
    private float packagingSpeed;
    [Tooltip("�� ������ ����")]
    [SerializeField]
    private MoneyStack moneyStack;

    [Space(10)]
    [Header("-Specs")]
    [Tooltip("��⿭ ���� Ʈ������ (�ش� Ʈ������ ���� z��)")]
    [SerializeField]
    private Transform waitingLineTr;
    [Tooltip("��⿭ �� ����")]
    [SerializeField]
    private float waitingLineSpacing;

    private Queue<Customer> waitingCustomerQueue;

    // 1���� �� �Ҵ�
    private Customer firstOrderCustomer = null;

    // �ֹ�ó�� �Ϸ� �� Invoke
    public UnityEvent OnProcessedOrder;

    [Space(10)]
    [Header("-Ballancing")]
    [Tooltip("�÷��̾ ��������� Ȯ��")]
    [SerializeField]
    private bool isInPlayer = false;

    private Coroutine packagingRoutine;

    private void Awake()
    {
        waitingCustomerQueue = new Queue<Customer>();
    }

    #region �÷��̾�-�� ��ȣ�ۿ�
    // �ֹ� ó��
    public void ProcessOrder()
    {
        // �÷��̾ ���� �������� ���� ���
        if (!isInPlayer || firstOrderCustomer == null)
            return;

        if (packagingRoutine != null)
            StopCoroutine(packagingRoutine);

        packagingRoutine = StartCoroutine(PackagingDelay());
    }
    #endregion

    #region �� ��ȣ�ۿ�

    // �ֹ� ��û
    public void RequestOrder(Customer customer)
    {
        // �켱������ ���� �ʴ� ��û�� �ź�
        if (waitingCustomerQueue.Peek() != customer)
            return;

        firstOrderCustomer = customer;

        // �÷��̾ ������ ��� �ֹ� ó��
        if (isInPlayer)
            ProcessOrder();
    }
    // �ֹ� ��⿭ ��ǥ ��ȯ (�� ��û : �� ����� -> ī����)
    public Vector3 GetWatingLine(Customer customer)
    {
        int count = waitingCustomerQueue.Count;
        waitingCustomerQueue.Enqueue(customer);
        // �ֹ�ó�� �����Ҵ�
        customer.orderTurn = count; 
        return waitingLineTr.position + (waitingLineTr.forward * waitingLineSpacing * count);
    }
    // �ٴ�� (ī���� -> ī����) 
    public Vector3 GetWatingLine(int order)
    {
        return waitingLineTr.position + (waitingLineTr.forward * waitingLineSpacing * (order));
    }   
    // �� ����
    public void PayMoney(int count)
    {
        moneyStack.SpawnMoney(count);
    }
    #endregion

    #region �÷��̾� ��ȣ�ۿ�
    public void EnterPlayer(PlayerItemController playerItemController)
    {
        isInPlayer = true;
       
        // ���� ������� ���� ���� ���
        if (waitingCustomerQueue.Count == 0 || firstOrderCustomer == null)
            return;

        // �ֹ� ó��
        ProcessOrder();
    }
    public void ExitPlayer()
    {
        isInPlayer = false;
    }
    #endregion

    // ������ ���� ������
    private IEnumerator PackagingDelay()
    {
        // ������� ����
        PaperBag paperBag = paperBagSpawner.Spawn() as PaperBag;
        ItemStack targetStack = firstOrderCustomer.ItemController.ItemStack;
        waitingCustomerQueue.Dequeue();

        while (targetStack.CurStackCount > 0)
        {
            Item getItem = targetStack.PopItem();
            // ������ ȿ�� ����
            yield return BazierCurve(getItem.transform, transform.position);
            // ������ ��Ȱ��ȭ
            getItem.Release();
        }

        // �ֹ��Ϸ� ó��
        // ������� ����
        firstOrderCustomer.SendItem(paperBag);
        // �ʱ�ȭ
        firstOrderCustomer = null;
        // ���� ���������� ������
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
            rate += Time.deltaTime * packagingSpeed; // ���� �ӵ� ����
            yield return null;
        }
        targetTransform.position = destination;
    }
}
