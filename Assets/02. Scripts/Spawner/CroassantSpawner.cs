using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 크로아상 스포너
/// </summary>
public class CroassantSpawner : PoolSpawner<Item>
{
    [Header("-Specs")]
    [Tooltip("스폰 물리력")]
    [SerializeField]
    private float spawnPower;

    [Header("-Ballancing")]
    [Tooltip("스폰 딜레이")]
    [SerializeField]
    private float spawnDelayTime;

    private Coroutine spawnRoutine = null;

    [Header("-Ballancing")]
    [Tooltip("크로아상 스택")]
    [SerializeField]
    private Stack<Croassant> croassantStack;
    public Stack<Croassant> CroassantStack { get { return croassantStack; } }

    private void Awake()
    {
        croassantStack = new Stack<Croassant>();
    }

    protected override IEnumerator InitDelay()
    {
        Debug.Log("InitDelay 실행");
        yield return base.InitDelay();
        spawnRoutine = StartCoroutine(SpawnRoutine());
    }

    // 오브젝트 재스폰 (이벤트 등록 메서드)
    public void Respawn()
    {
        curSpawnCount--;
        if (spawnRoutine == null)
            spawnRoutine = StartCoroutine(SpawnRoutine());
    }
    [ContextMenu("Spawn")]
    protected override void Spawn()
    {
        Croassant inst = PoolManager.Instance.GetPool(objectPrefab, Vector3.zero, Quaternion.identity) as Croassant;
        if (inst == null)
        {
            Debug.Log("풀에 등록되지 않은 오브젝트 : CroassantSpawner");
            return;
        }
        inst.transform.position = transform.position;
        inst.transform.rotation = transform.rotation;
        // 스폰 시 물리력 적용
        inst.Rigidbody.AddForce(transform.forward * spawnPower, ForceMode.Impulse);
        croassantStack.Push(inst);
    }
    private IEnumerator SpawnRoutine()
    {
        while (curSpawnCount < maxSpawnCount)
        {
            yield return new WaitForSeconds(spawnDelayTime);
            Spawn();
            curSpawnCount++;
        }

        // 코루틴 종료
        spawnRoutine = null;
    }
}
