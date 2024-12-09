using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : Singleton<ScoreManager>
{
    // ���� �������� ��
    private int curMoney;
    public int CurMoney {get { return curMoney; } }

    // �� ����, ���� �� Invoke
    public UnityEvent<int> OnChangeScore; 

    // �� ����
    public void GetMoney(int value)
    {
        Debug.Log(CurMoney);
        curMoney += value;
        OnChangeScore?.Invoke(curMoney);
    }
    // �� ����
    public void PayMoney(int value)
    {
        curMoney -= value;
        OnChangeScore?.Invoke(curMoney);
    }
}

