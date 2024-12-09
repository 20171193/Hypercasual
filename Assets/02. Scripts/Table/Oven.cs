using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oven : Table, IPlayerInteractable
{
    [Header("-Components")]
    [SerializeField]
    private CroassantSpawner spawner;

    private Coroutine croassantPopRoutine;

    [Header("-Ballancing")]
    [Tooltip("�÷��̾ ��������� Ȯ��")]
    [SerializeField]
    private bool isInPlayer = false;
    private PlayerItemController playerItemController = null;

    private void Awake()
    {
        interactType = InteractType.Player;
    }

    private void OnEnable()
    {
        // ũ�ξƻ� ���� �ݹ� ���
        spawner.OnSpawnCroassant.AddListener(SendCroassant);
    }
    private void OnDisable()
    {
        // ũ�ξƻ� ���� �ݹ� ���
        spawner.OnSpawnCroassant.RemoveListener(SendCroassant);
    }

    // ũ�ξƻ��� ������ ��� �ݹ� (�÷��̾ ������� ��� ���� �뵵)
    private void SendCroassant()
    {
        // �÷��̾ ������� �ƴ� ���
        if (!isInPlayer || playerItemController == null)
            return;

        if (croassantPopRoutine == null)
            croassantPopRoutine = StartCoroutine(CroassantPopRoutine(playerItemController.ItemStack));
    }

    public void EnterPlayer(PlayerItemController targetController)
    {
        if (targetController.ItemStack.isFull)
            return;

        isInPlayer = true;
        playerItemController = targetController;

        if (croassantPopRoutine == null)
            croassantPopRoutine = StartCoroutine(CroassantPopRoutine(targetController.ItemStack));
    }
    public void ExitPlayer()
    {
        isInPlayer = false;
    }

    // �� ���� ������ ��ƾ
    private IEnumerator CroassantPopRoutine(ItemStack targetStack)
    {
        while (!targetStack.isFull && spawner.CroassantStack.Count > 0)
        {
            // �÷��̾� �������� �̵�
            targetStack.PushItem(spawner.CroassantStack.Pop());
            // ������ ���
            spawner.Respawn();
            yield return new WaitForSeconds(0.1f);
        }
        croassantPopRoutine = null;
    }
}
