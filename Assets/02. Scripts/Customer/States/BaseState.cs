using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 모든 상태의 기본 메서드 모음
public class BaseState
{
    public virtual void Enter(){ }
    public virtual void Update(){ }
    public virtual void Exit(){ }
}

public class CustomerState : BaseState
{
    protected Customer owner;
}