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
    [Tooltip("플레이어가 대기중인지 확인")]
    [SerializeField]
    private bool isInPlayer = false;
    private PlayerItemController playerItemController = null;

    private void Awake()
    {
        interactType = InteractType.Player;
    }

    private void OnEnable()
    {
        // 크로아상 스폰 콜백 등록
        spawner.OnSpawnCroassant.AddListener(SendCroassant);
    }
    private void OnDisable()
    {
        // 크로아상 스폰 콜백 등록
        spawner.OnSpawnCroassant.RemoveListener(SendCroassant);
    }

    // 크로아상이 스폰된 경우 콜백 (플레이어가 대기중인 경우 전달 용도)
    private void SendCroassant()
    {
        // 플레이어가 대기중이 아닐 경우
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

    // 빵 습득 딜레이 루틴
    private IEnumerator CroassantPopRoutine(ItemStack targetStack)
    {
        while (!targetStack.isFull && spawner.CroassantStack.Count > 0)
        {
            // 플레이어 스택으로 이동
            targetStack.PushItem(spawner.CroassantStack.Pop());
            // 리스폰 등록
            spawner.Respawn();
            yield return new WaitForSeconds(0.1f);
        }
        croassantPopRoutine = null;
    }
}
