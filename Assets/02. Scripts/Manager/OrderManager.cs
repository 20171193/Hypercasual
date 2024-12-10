using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 주문요청
/// </summary>
[Serializable]
public struct RequestCroassant
{
    [Tooltip("주문 고객")]
    public Customer customer;
    [Tooltip("요청 개수")]
    public int count;
    public RequestCroassant(Customer customer, int count)
    {
        this.customer = customer;
        this.count = count;
    }
}

public enum OrderType
{
    TakeOut,    // 포장
    DineIn      // 매장
}

/// <summary>
/// 고객 생성 시 할당할 주문
/// </summary>
[Serializable]
public struct CustomerOrder
{
    [Tooltip("주문 타입")]
    public OrderType orderType; // 주문 타입
    [Tooltip("주문 개수")]
    public int orderCount;      // 주문할 개수

    public CustomerOrder(OrderType orderType, int orderCount)
    {
        this.orderType = orderType;
        this.orderCount = orderCount;
    }
}
public class OrderManager : Singleton<OrderManager>
{
    [Header("테이블 객체 할당")]
    public Basket basket;
    public Oven oven;
    public Counter counter;
    public DineInTable dineIn;

    [Space(10)]
    [Header("출입구 할당")]
    public Transform entranceTr;
    [Header("빵 보관소 경유지 할당")]
    public Transform basketTransitTr;
    [Header("카운터 경유지 할당")]
    public Transform counterTransitTr;
    /// <summary>
    /// 테이블 타입
    /// </summary>
    public enum TableType
    {
        Basket = 0,
        Oven,
        Counter,
        Dining
    }

    /// <summary>
    /// 테이블 객체 요청
    /// </summary>
    public Table GetTable(TableType tableType)
    {
        switch (tableType)
        {
            case TableType.Basket:
                return basket;
            case TableType.Oven:
                return oven;
            case TableType.Counter:
                return counter;
            case TableType.Dining:
            default:
                return null;
        }
    }

    /// <summary>
    /// 주문 반환
    /// </summary>
    public CustomerOrder GetOrder()
    {
        int count = UnityEngine.Random.Range(1, 4);

        // 주문개수가 1개라면 먹고가기
        if (count == 1)
            return new CustomerOrder(OrderType.DineIn, count);
        else
            return new CustomerOrder(OrderType.TakeOut, count);
    }
}
