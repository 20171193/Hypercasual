using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oven : MonoBehaviour, IStackInteractable
{
    [Header("-Components")]
    [SerializeField]
    private CroassantSpawner spawner;

    private Coroutine croassantPopRoutine;

    // IStackInteractable 인터페이스 재정의
    public bool InteractStack(ItemStack targetStack)
    {
        if (targetStack.isFull && targetStack.Onwer != ItemStack.StackOwner.Player)
            return false;

        if (croassantPopRoutine == null)
            croassantPopRoutine = StartCoroutine(CroassantPopRoutine(targetStack));

        return true;
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
