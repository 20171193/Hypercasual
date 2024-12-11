using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    [Tooltip("크로아상 스폰 시 Invoke")]
    public UnityEvent OnSpawnCroassant; 

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

    // 오브젝트 재스폰 (콜백 메서드)
    public void Respawn()
    {
        curSpawnCount--;
        if (spawnRoutine == null)
            spawnRoutine = StartCoroutine(SpawnRoutine());
    }
    [ContextMenu("Spawn")]
    public override PooledObject Spawn()
    {
        Croassant inst = base.Spawn() as Croassant;
        if (inst == null)
        {
            Debug.Log("형변환 오류 : PooledObject to Croassant");
            return null;
        }
        inst.transform.position = transform.position;
        inst.transform.rotation = transform.rotation;
        // 스폰 시 물리력 적용
        inst.Rigidbody.AddForce(transform.forward * spawnPower, ForceMode.Impulse);
        croassantStack.Push(inst);
        OnSpawnCroassant?.Invoke();
        return inst;
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
