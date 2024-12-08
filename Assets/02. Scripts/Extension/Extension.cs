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
}
