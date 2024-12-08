using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oven : MonoBehaviour, IStackInteractable
{
    [Header("-Components")]
    [SerializeField]
    private CroassantSpawner spawner;

    private Coroutine croassantPopRoutine;

    // IStackInteractable �������̽� ������
    public bool InteractStack(ItemStack targetStack)
    {
        if (targetStack.isFull && targetStack.Onwer != ItemStack.StackOwner.Player)
            return false;

        if (croassantPopRoutine == null)
            croassantPopRoutine = StartCoroutine(CroassantPopRoutine(targetStack));

        return true;
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
