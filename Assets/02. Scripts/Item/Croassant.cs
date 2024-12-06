using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ũ�ξƻ� ������
/// </summary>
public class Croassant : Item
{
    // ũ�ξƻ� ����
    public enum State
    {
        Pooled,             // ������Ʈ Ǯ�� ������� ���� (�ʱ�ȭ)
        OnSpawner,          //                 ������ ���� (������ ����)
        OnPlayerStack,      //   �÷��̾� ���ÿ� ���� ���� (������ ����) 
        OnCustomerStack,    //       �� ���ÿ� ���� ���� (������ ����)
        OnBasketStack       //     ������ ���ÿ� ���� ���� (������ ����)
    }

    [Header("-Components")]
    [SerializeField]
    private Rigidbody rigid;

    [Header("-Specs")]
    [Tooltip("������ �� Ƣ����� ��")]
    [SerializeField]
    private float spawnPower;

    [Header("-Ballancing")]
    [SerializeField]
    private State curState = State.Pooled;
    public State CurState { get { return curState; } }

    public void ChangeState(State state)
    {
        switch (state)
        {
            case State.Pooled:
                {
                    // ������� ����
                    rigid.isKinematic = false;
                    pooler.ReturnPool(this);
                    break;
                }
            case State.OnSpawner:
                {
                    // ������� ����
                    rigid.isKinematic = false;
                    // ���� �� Ƣ����� �� ����
                    rigid.AddForce(transform.forward * spawnPower, ForceMode.Impulse);
                    break;
                }
            case State.OnPlayerStack:
            case State.OnCustomerStack:
            case State.OnBasketStack:
                {
                    // ������� ����
                    rigid.isKinematic = true;
                    break;
                }
            default:
                break;
        }

        curState = state;
    }
}
