using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;
using UnityEditor.PackageManager.Requests;

// �� ������� ��⿭�� ������� ��� �� ���� ��û

/// <summary>
/// �� ����� ���̺�, �� ���� �� �ֹ� �Ҵ�
/// </summary>
public class Basket : Table, IPlayerInteractable
{
    [Header("-Components")]
    [SerializeField]
    private ItemStack basketStack;

    [SerializeField]
    private CustomerSpawner customerSpawner;

    [Header("-Specs")]
    [Tooltip("�ֹ� ��⿭ ��ġ")]
    [SerializeField]
    private Transform[] waitingLineTransform;

    [Header("-Ballancing")]
    [Tooltip("�ֹ���û ť")]
    [SerializeField]
    private Queue<Customer.RequestCroassant> requests;
    [Tooltip("��⿭ �� �� (�ֹ� ��⿭�� 1:1)")]
    [SerializeField]
    private Customer[] inCustomers;
    
    [Tooltip("�÷��̾� ������ ��Ʈ�ѷ� (Ʈ���� �� �Ҵ�)")]
    [SerializeField]
    private PlayerItemController playerItemController = null;
    [Tooltip("�÷��̾ ��������� Ȯ��")]
    [SerializeField]
    private bool isInPlayer = false;

    private Coroutine croassantPushRoutine;
    private Coroutine croassantPopRoutine;
    private Coroutine spawnCustomerRoutine;

    private void Awake()
    {
        requests = new Queue<Customer.RequestCroassant>();

        // ��⿭ ���� ��ŭ �迭�Ҵ� 
        inCustomers = new Customer[waitingLineTransform.Length];

        customerSpawner.OnInitSpanwer.AddListener(SpawnCustomer);
    }
    private void OnEnable()
    {
        // �ֹ�ó�� �ݹ� ���
        basketStack.OnStackedItem.AddListener(ProcessOrder);
    }
    private void OnDisable()
    {
        basketStack.OnStackedItem.RemoveListener(ProcessOrder);
    }

    // ������ ��û
    private void SpawnCustomer()
    {
        if(spawnCustomerRoutine != null)
            StopCoroutine(spawnCustomerRoutine);

        spawnCustomerRoutine = StartCoroutine(SpawnCustomerRoutine());
    }

    // �� ��⿭ ã��
    private bool GetWaitingLine(out Vector3 waitingLinePosition, out int waitingLineIndex)
    {
        waitingLinePosition = Vector3.zero;
        waitingLineIndex = -1;
        for (int i = 0; i < inCustomers.Length; i++)
        {
            if (inCustomers[i] == null)
            {
                // ��⿭ ��ġ �Ҵ� (out ��ȯ)
                waitingLinePosition = waitingLineTransform[i].position;
                // ��⿭ �ε��� �Ҵ�
                waitingLineIndex = i;
                return true;
            }
        }
        return false;
    }

    private IEnumerator SpawnCustomerRoutine()
    {
        // ��⿭�� ���� �������� ����
        while (GetWaitingLine(out Vector3 waitingLinePosition, out int watingLineIndex))
        {
            Customer customer = customerSpawner.Spawn() as Customer;
            // ��⿭�� �� �Ҵ�
            inCustomers[watingLineIndex] = customer;
            // ������ �Ҵ�
            customer.destination = waitingLinePosition;
            // ��⿭ �ε��� �Ҵ�
            customer.orderTurn = watingLineIndex;
            // �� ���� ���·� ����
            customer.FSM.ChangeState("Selecting");
            yield return new WaitForSeconds(1f);
        }
        spawnCustomerRoutine = null;
    }

    #region �� ��ȣ�ۿ�
    // �ֹ� ó�� (OnStackedItem Invoke �� �߻�)
    private void ProcessOrder()
    {
        // �ֹ� ��û�� ���ų� ���ÿ� ���� �������� ���� ��� return
        if (requests.Count == 0 || basketStack.CurStackCount == 0)
            return;

        // �÷��̾ ������� ��� �� ����
        if (isInPlayer && playerItemController != null)
            croassantPushRoutine = StartCoroutine(CroassantPushRoutine(playerItemController.ItemStack));

        // �ֹ� ����
        if (croassantPopRoutine == null)
            croassantPopRoutine = StartCoroutine(CroassantPopRoutine());
    }
    // �ֹ� ��û 
    public void RequestOrder(Customer.RequestCroassant request)
    {
        // �ֹ� ��⿭ ť �Ҵ�
        requests.Enqueue(request);
        // �ֹ� ó��
        ProcessOrder();
    }
    // �� ���� ������ ��ƾ 
    private IEnumerator CroassantPopRoutine()
    {
        while(requests.Count > 0)
        {
            // ������ ��û �Ҵ�
            Customer.RequestCroassant curRequest = requests.Peek();
            // ���� �Ҵ�� ��⿭ �ε��� Ȯ��
            int index = curRequest.customer.orderTurn;

            // 0.1�� ������ ���� ����
            while(basketStack.CurStackCount > 0 && curRequest.count > 0)
            {
                curRequest.customer.SendItem(basketStack.PopItem());
                curRequest.count--;
                yield return new WaitForSeconds(0.1f);
            }

            // �ֹ���û�� ��� ó���� ���
            if (curRequest.count == 0)
            {
                inCustomers[index] = null;
                requests.Dequeue();
                // �� �� ����
                SpawnCustomer();
            }
            else
                yield return null;
        }

        croassantPopRoutine = null;
    }
    #endregion

    #region �÷��̾� ��ȣ�ۿ�
    // �� ���� 
    public void EnterPlayer(PlayerItemController targetController)
    {
        if (targetController.ItemStack.CurStackCount == 0)
            return;

        isInPlayer = true;
        // ���� ������ ������ ����
        if (basketStack.isFull)
        {
            // �÷��̾� ��Ʈ�ѷ� ��⿭�� �Ҵ�
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
    // �� ���� ������ ��ƾ 
    private IEnumerator CroassantPushRoutine(ItemStack targetStack)
    {
        // �÷��̾� ���� ��⿭ �Ҵ�����
        while (targetStack.CurStackCount > 0 && !basketStack.isFull)
        {
            // �÷��̾ Ż���� ��� ��ƾ Ż��
            if (!isInPlayer)
            {
                croassantPushRoutine = null;
                yield break;
            }

            // �� ����� �������� �̵�
            basketStack.PushItem(targetStack.PopItem());
            yield return new WaitForSeconds(0.1f);
        }

        croassantPushRoutine = null;
    }
    #endregion
}
