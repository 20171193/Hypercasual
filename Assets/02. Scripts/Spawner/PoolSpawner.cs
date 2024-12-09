using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Ǯ���� ������ ������
/// </summary>
public abstract class PoolSpawner<TObject> : MonoBehaviour where TObject : PooledObject
{
    [Header("-Components")]
    [SerializeField]
    protected TObject objectPrefab;

    [Space(10)]
    [Header("-Specs")]
    [Tooltip("���� ����")]
    [SerializeField]
    protected int maxSpawnCount = 0;
    public int MaxSpawnCount { get { return maxSpawnCount; } }
    [Tooltip("Ǯ ������")]
    [SerializeField]
    protected int poolSize;
    [Tooltip("Ǯ �뷮")]
    [SerializeField]
    protected int poolCapacity;

    [Header("-Ballancing")]
    protected int curSpawnCount = 0;
    public int CurSpawnCount { get { return curSpawnCount; } }

    //[Space(10)]
    //[Header("-Ballancing")]
    //[Tooltip("������ ������ �ν��Ͻ� ID")]
    protected Coroutine initDelay;

    // ������ �ε�(�ʱ�ȭ) �Ϸ� �� Invoke
    public UnityEvent OnInitSpanwer;

    protected virtual void OnEnable()
    {
        initDelay = StartCoroutine(InitDelay());
    }

    // Ǯ�Ŵ��� �ʱ�ȭ ��� �� ����
    protected virtual IEnumerator InitDelay()
    {
        yield return new WaitForSeconds(0.1f);
        // ������Ʈ Ǯ ������û
        PoolManager.Instance.CreatePool(objectPrefab, poolSize, poolCapacity);
        OnInitSpanwer?.Invoke();
    }
    public virtual PooledObject Spawn()
    {
        PooledObject inst = PoolManager.Instance.GetPool(objectPrefab, Vector3.zero, Quaternion.identity);
        if (inst == null)
        {
            Debug.Log("Ǯ�� ��ϵ��� ���� ������Ʈ : Customer");
            return null;
        }
        return inst;
    }
}
