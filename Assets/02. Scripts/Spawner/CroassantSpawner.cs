using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // ������Ʈ �罺�� (�̺�Ʈ ��� �޼���)
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
            Debug.Log("Ǯ�� ��ϵ��� ���� ������Ʈ : CroassantSpawner");
            return;
        }
        inst.transform.position = transform.position;
        inst.transform.rotation = transform.rotation;
        // ���� �� ������ ����
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

        // �ڷ�ƾ ����
        spawnRoutine = null;
    }
}
