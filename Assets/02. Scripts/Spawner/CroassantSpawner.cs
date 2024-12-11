using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ũ�ξƻ� ������
/// </summary>
public class CroassantSpawner : PoolSpawner<Item>
{
    [Header("-Specs")]
    [Tooltip("���� ������")]
    [SerializeField]
    private float spawnPower;

    [Header("-Ballancing")]
    [Tooltip("���� ������")]
    [SerializeField]
    private float spawnDelayTime;

    private Coroutine spawnRoutine = null;

    [Header("-Ballancing")]
    [Tooltip("ũ�ξƻ� ����")]
    [SerializeField]
    private Stack<Croassant> croassantStack;
    public Stack<Croassant> CroassantStack { get { return croassantStack; } }

    [Tooltip("ũ�ξƻ� ���� �� Invoke")]
    public UnityEvent OnSpawnCroassant; 

    private void Awake()
    {
        croassantStack = new Stack<Croassant>();
    }

    protected override IEnumerator InitDelay()
    {
        Debug.Log("InitDelay ����");
        yield return base.InitDelay();
        spawnRoutine = StartCoroutine(SpawnRoutine());
    }

    // ������Ʈ �罺�� (�ݹ� �޼���)
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
            Debug.Log("����ȯ ���� : PooledObject to Croassant");
            return null;
        }
        inst.transform.position = transform.position;
        inst.transform.rotation = transform.rotation;
        // ���� �� ������ ����
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

        // �ڷ�ƾ ����
        spawnRoutine = null;
    }
}
