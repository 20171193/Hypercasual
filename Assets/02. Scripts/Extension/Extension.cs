using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// 확장 메서드
public static class Extension
{
    // 레이어마스크가 해당 레이어를 포함하고 있는지 체크
    public static bool Contain(this LayerMask layerMask, int layer)
    {
        return ((1 << layer) & layerMask) != 0;
    }

    // 일정시간(초) 딜레이 이후 액션 실행
    public static IEnumerator DelayAction(float delayTime, UnityAction action)
    {
        yield return new WaitForSeconds(delayTime);
        action?.Invoke();
    }

    /// <summary>
    /// 베지어 곡선 
    /// </summary>
    /// <param name="targetTransform"> 이동할 트랜스폼 (시작위치 설정 및 이동) </param>
    /// <param name="destination"> 목적지 </param>
    /// <param name="speed"> 이동 속도 </param>
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
            rate += Time.deltaTime * speed; // 습득 속도 적용
            yield return null;
        }
        targetTransform.position = destination;
    }
}
