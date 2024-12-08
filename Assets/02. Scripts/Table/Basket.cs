using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;

/// <summary>
/// 빵 주문요청
/// </summary>
[Serializable]
public struct RequestCroassant
{
    [Tooltip("주문 요청자")]
    public Customer customer;
    [Tooltip("요청 개수")]
    public int count;
    public RequestCroassant(Customer customer, int count)
    {
        this.customer = customer;
        this.count = count;
    }
}

/// <summary>
/// 빵 저장소 테이블
/// </summary>
public class Basket : MonoBehaviour, IStackInteractable
{
    [Header("-Components")]
    [SerializeField]
    private ItemStack itemStack;

    [Header("-Ballancing")]
    [Tooltip("주문요청 큐")]
    [SerializeField]
    private Queue<RequestCroassant> requests;

    [Tooltip("현재 진행중인 요청")]
    [SerializeField]
    private RequestCroassant? curRequest = null;
    
    private Coroutine croassantPushRoutine;

    private void Awake()
    {
        itemStack.OnStackedItem.AddListener(ProcessOrder);
    }

    // 주문 처리
    private void ProcessOrder(Item item)
    {
        // 현재 처리중인 주문이 없을 경우
        if (curRequest == null)
            return;

        // 주문처리

    }
    // 주문 요청 (고객)
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
    // 빵 저장 딜레이 루틴
    private IEnumerator CroassantPushRoutine(ItemStack targetStack)
    {
        while (targetStack.CurStackCount > 0 && !itemStack.isFull)
        {
            // 빵 저장소 스택으로 이동
            itemStack.PushItem(targetStack.PopItem());
            yield return new WaitForSeconds(0.1f);
        }

        croassantPushRoutine = null;
    }
}
