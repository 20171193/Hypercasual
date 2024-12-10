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

    /// <summary>
    /// ������ � 
    /// </summary>
    /// <param name="targetTransform"> �̵��� Ʈ������ (������ġ ���� �� �̵�) </param>
    /// <param name="destination"> ������ </param>
    /// <param name="speed"> �̵� �ӵ� </param>
    /// <returns></returns>
    public static IEnumerator BazierCurve(Transform targetTransform, Vector3 destination, float speed)
    {
        Vector3 startPos = targetTransform.position;
        Vector3 midPos = Vector3.Lerp(startPos, destination, 0.5f) + Vector3.up * 3f;
        float rate = 0f;
        while (rate < 1f)
        {
            Vector3 p1 = Vector3.Lerp(startPos, midPos, rate);
            Vector3 p2 = Vector3.Lerp(midPos, destination, rate);
            targetTransform.position = Vector3.Lerp(p1, p2, rate);
            rate += Time.deltaTime * speed; // ���� �ӵ� ����
            yield return null;
        }
        targetTransform.position = destination;
    }
}
