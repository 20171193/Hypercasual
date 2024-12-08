using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;

/// <summary>
/// �� �ֹ���û
/// </summary>
[Serializable]
public struct RequestCroassant
{
    [Tooltip("�ֹ� ��û��")]
    public Customer customer;
    [Tooltip("��û ����")]
    public int count;
    public RequestCroassant(Customer customer, int count)
    {
        this.customer = customer;
        this.count = count;
    }
}

/// <summary>
/// �� ����� ���̺�
/// </summary>
public class Basket : MonoBehaviour, IStackInteractable
{
    [Header("-Components")]
    [SerializeField]
    private ItemStack itemStack;

    [Header("-Ballancing")]
    [Tooltip("�ֹ���û ť")]
    [SerializeField]
    private Queue<RequestCroassant> requests;

    [Tooltip("���� �������� ��û")]
    [SerializeField]
    private RequestCroassant? curRequest = null;
    
    private Coroutine croassantPushRoutine;

    private void Awake()
    {
        itemStack.OnStackedItem.AddListener(ProcessOrder);
    }

    // �ֹ� ó��
    private void ProcessOrder(Item item)
    {
        // ���� ó������ �ֹ��� ���� ���
        if (curRequest == null)
            return;

        // �ֹ�ó��

    }
    // �ֹ� ��û (��)
    public void RequestOrder(Customer customer, int count)
    {
        RequestCroassant request = new RequestCroassant(customer, count);

        if (curRequest == null)
        { 
            curRequest = request;
        }
        else
        {
            requests.Enqueue(request);
        }
    }
    public bool InteractStack(ItemStack targetStack)
    {
        if (itemStack.isFull || targetStack.CurStackCount == 0)
            return false;

        if(croassantPushRoutine == null)
            croassantPushRoutine = StartCoroutine(CroassantPushRoutine(targetStack));

        return true;
    }
    // �� ���� ������ ��ƾ
    private IEnumerator CroassantPushRoutine(ItemStack targetStack)
    {
        while (targetStack.CurStackCount > 0 && !itemStack.isFull)
        {
            // �� ����� �������� �̵�
            itemStack.PushItem(targetStack.PopItem());
            yield return new WaitForSeconds(0.1f);
        }

        croassantPushRoutine = null;
    }
}
