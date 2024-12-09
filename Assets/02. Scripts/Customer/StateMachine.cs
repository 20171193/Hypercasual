using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<TOwner>
{
    // 상태머신 소유자
    private TOwner owner;
    public TOwner Owner { get { return owner; } }

    // 현재 상태
    private string curState;
    public string CurState { get { return curState; } }

    // 딕셔너리를 통한 상태 검색
    private Dictionary<string, BaseState> stateDic;
    public Dictionary<string, BaseState> StateDic { get { return stateDic; } }

    public StateMachine(TOwner owner)
    {
        this.owner = owner;
        stateDic = new Dictionary<string, BaseState>();
    }

    public void AddState(string key, BaseState value)
    {
        stateDic.Add(key, value);
    }

    // 초기 상태 지정
    public void Init(string entry)
    {
        curState = entry;
        stateDic[entry].Enter();
    }

    // 상태 전이
    public void ChangeState(string nextState)
    {
        // 현재 상태 탈출 -> 다음상태 진입
        // 이전 상태의 Exit이 먼저 호출되고 다음 상태의 Enter호출
        stateDic[curState].Exit();
        curState = nextState;
        stateDic[curState].Enter();
    }

    // 각 상태 실행
    public void Update()
    {
        stateDic[curState].Update();
    }
}
