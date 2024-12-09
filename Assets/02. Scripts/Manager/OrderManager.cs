using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ֹ���û
/// </summary>
[Serializable]
public struct RequestCroassant
{
    [Tooltip("�ֹ� ��")]
    public Customer customer;
    [Tooltip("��û ����")]
    public int count;
    public RequestCroassant(Customer customer, int count)
    {
        this.customer = customer;
        this.count = count;
    }
}

public enum OrderType
{
    TakeOut,    // ����
    DineIn      // ����
}

/// <summary>
/// �� ���� �� �Ҵ��� �ֹ�
/// </summary>
[Serializable]
public struct CustomerOrder
{
    [Tooltip("�ֹ� Ÿ��")]
    public OrderType orderType; // �ֹ� Ÿ��
    [Tooltip("�ֹ� ����")]
    public int orderCount;      // �ֹ��� ����

    public CustomerOrder(OrderType orderType, int orderCount)
    {
        this.orderType = orderType;
        this.orderCount = orderCount;
    }
}
public class OrderManager : Singleton<OrderManager>
{
    [Header("���̺� ��ü �Ҵ�")]
    [SerializeField]
    public Basket basket;
    [SerializeField]
    public Oven oven;
    [SerializeField]
    public Counter counter;
    /// <summary>
    /// ���̺� Ÿ��
    /// </summary>
    public enum TableType
    {
        Basket = 0,
        Oven,
        Counter,
        Dining
    }

    /// <summary>
    /// ���̺� ��ü ��û
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
}
