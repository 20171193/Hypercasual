using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 활성화 시 오브젝트 크기 변경 효과적용
/// </summary>
public class PopObject : MonoBehaviour
{
    [Header("-Specs")]
    [Tooltip("최소 크기 : 이벤트 시작 크기")]
    [SerializeField]
    private Vector3 minScale;
    [Tooltip("최대 크기 : 이벤트 크기")]
    [SerializeField]
    private Vector3 maxScale;
    [Tooltip("사이즈 업 시간")]
    [SerializeField]
    private float sizeUpTime;
    [Tooltip("사이즈 원상복구 시간")]
    [SerializeField]
    private float sizeResetTime;


    private Coroutine popRoutine;

    [Space(10)]
    [Header("-Ballancing")]
    [SerializeField]
    private Vector3 originScale;
    [SerializeField]
    private float totalRoutineTime;

    private void Awake()
    {
        // 루틴 총 시간 할당
        totalRoutineTime = sizeUpTime + sizeResetTime;
        // 초기 사이즈 할당
        originScale = transform.localScale;
    }

    private void OnEnable()
    {
        popRoutine = StartCoroutine(PopRoutine());
    }

    // 크기 변경 효과적용
    private IEnumerator PopRoutine()
    {
        float rate = 0f;
        float curTime = 0f;
        
        transform.localScale = minScale;
        while(rate < 1f)
        {
            // 사이즈 업 (Vector3.zero -> 최대 사이즈)
            if(curTime < sizeUpTime)
            {
                transform.localScale = Vector3.Lerp(minScale, maxScale, curTime / sizeUpTime);
                Debug.Log($"Size Up Rate : {curTime / sizeUpTime}");
            }
            // 사이즈 리셋 (최대 사이즈 -> 초기 사이즈)
            else
            {
                transform.localScale = Vector3.Lerp(maxScale, originScale, (curTime - sizeUpTime) / sizeResetTime);
                Debug.Log($"Size Reset Rate : {(curTime - sizeUpTime) / sizeResetTime}");
            }

            rate += Time.deltaTime / totalRoutineTime;
            curTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = originScale;
    }
}
