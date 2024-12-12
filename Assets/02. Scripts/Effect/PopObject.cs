using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ȱ��ȭ �� ������Ʈ ũ�� ���� ȿ������
/// </summary>
public class PopObject : MonoBehaviour
{
    [Header("-Specs")]
    [Tooltip("�ּ� ũ�� : �̺�Ʈ ���� ũ��")]
    [SerializeField]
    private Vector3 minScale;
    [Tooltip("�ִ� ũ�� : �̺�Ʈ ũ��")]
    [SerializeField]
    private Vector3 maxScale;
    [Tooltip("������ �� �ð�")]
    [SerializeField]
    private float sizeUpTime;
    [Tooltip("������ ���󺹱� �ð�")]
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
        // ��ƾ �� �ð� �Ҵ�
        totalRoutineTime = sizeUpTime + sizeResetTime;
        // �ʱ� ������ �Ҵ�
        originScale = transform.localScale;
    }

    private void OnEnable()
    {
        popRoutine = StartCoroutine(PopRoutine());
    }

    // ũ�� ���� ȿ������
    private IEnumerator PopRoutine()
    {
        float rate = 0f;
        float curTime = 0f;
        
        transform.localScale = minScale;
        while(rate < 1f)
        {
            // ������ �� (Vector3.zero -> �ִ� ������)
            if(curTime < sizeUpTime)
            {
                transform.localScale = Vector3.Lerp(minScale, maxScale, curTime / sizeUpTime);
                Debug.Log($"Size Up Rate : {curTime / sizeUpTime}");
            }
            // ������ ���� (�ִ� ������ -> �ʱ� ������)
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
