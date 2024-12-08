using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Ȯ�� �޼���
public static class Extension
{
    // ���̾��ũ�� �ش� ���̾ �����ϰ� �ִ��� üũ
    public static bool Contain(this LayerMask layerMask, int layer)
    {
        return ((1 << layer) & layerMask) != 0;
    }

    // �����ð�(��) ������ ���� �׼� ����
    public static IEnumerator DelayAction(float delayTime, UnityAction action)
    {
        yield return new WaitForSeconds(delayTime);
        action?.Invoke();
    }
}
