using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 풀링된 아이템 생성기
/// </summary>
public abstract class PoolSpawner<TObject> : MonoBehaviour where TObject : PooledObject
{
    [Header("-Components")]
    [SerializeField]
    protected TObject objectPrefab;

    [Space(10)]
    [Header("-Specs")]
    [Tooltip("스폰 개수")]
    [SerializeField]
    protected int maxSpawnCount = 0;
    public int MaxSpawnCount { get { return maxSpawnCount; } }
    [Tooltip("풀 사이즈")]
    [SerializeField]
    protected int poolSize;
    [Tooltip("풀 용량")]
    [SerializeField]
    protected int poolCapacity;

    [Header("-Ballancing")]
    protected int curSpawnCount = 0;
    public int CurSpawnCount { get { return curSpawnCount; } }

    //[Space(10)]
    //[Header("-Ballancing")]
    //[Tooltip("아이템 프리팹 인스턴스 ID")]
    protected Coroutine initDelay;

    // 스포너 로딩(초기화) 완료 시 Invoke
    public UnityEvent OnInitSpanwer;

    protected virtual void OnEnable()
    {
        initDelay = StartCoroutine(InitDelay());
    }

    // 풀매니저 초기화 대기 후 실행
    protected virtual IEnumerator InitDelay()
    {
        yield return new WaitForSeconds(0.1f);
        // 오브젝트 풀 생성요청
        PoolManager.Instance.CreatePool(objectPrefab, poolSize, poolCapacity);
        OnInitSpanwer?.Invoke();
    }
    public virtual PooledObject Spawn()
    {
        PooledObject inst = PoolManager.Instance.GetPool(objectPrefab, Vector3.zero, Quaternion.identity);
        if (inst == null)
        {
            Debug.Log("풀에 등록되지 않은 오브젝트 : Customer");
            return null;
        }
        return inst;
    }
}
