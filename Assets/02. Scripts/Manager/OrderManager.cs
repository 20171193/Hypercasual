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
    public Basket basket;
    public Oven oven;
    public Counter counter;
    public DineInTable dineIn;

    [Space(10)]
    [Header("���Ա� �Ҵ�")]
    public Transform entranceTr;
    [Header("�� ������ ������ �Ҵ�")]
    public Transform basketTransitTr;
    [Header("ī���� ������ �Ҵ�")]
    public Transform counterTransitTr;
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

    /// <summary>
    /// �ֹ� ��ȯ
    /// </summary>
    public CustomerOrder GetOrder()
    {
        int count = UnityEngine.Random.Range(1, 4);

        // �ֹ������� 1����� �԰���
        if (count == 1)
            return new CustomerOrder(OrderType.DineIn, count);
        else
            return new CustomerOrder(OrderType.TakeOut, count);
    }
}
