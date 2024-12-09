using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : Singleton<ScoreManager>
{
    // ÇöÀç º¸À¯ÁßÀÎ µ·
    private int curMoney;
    public int CurMoney {get { return curMoney; } }

    // µ· ÁöºÒ, ½Àµæ ½Ã Invoke
    public UnityEvent<int> OnChangeScore; 

    // µ· ½Àµæ
    public void GetMoney(int value)
    {
        Debug.Log(CurMoney);
        curMoney += value;
        OnChangeScore?.Invoke(curMoney);
    }
    // µ· ÁöºÒ
    public void PayMoney(int value)
    {
        curMoney -= value;
        OnChangeScore?.Invoke(curMoney);
    }
}

