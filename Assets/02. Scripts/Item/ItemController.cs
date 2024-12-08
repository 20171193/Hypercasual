using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ ��Ʈ�ѷ� (��, �÷��̾�)
/// </summary>
public class ItemController : MonoBehaviour
{
    [Header("-Components")]
    [Tooltip("������ ����")]
    [SerializeField]
    private ItemStack itemStack;

    private void OnTriggerEnter(Collider other)
    {
        // �����۰� �浹 üũ
        if(LayerMaskMap.Instance.itemLM.Contain(other.gameObject.layer))
        {
            Item item = other.GetComponent<Item>();

            if(item == null)
            {
                Debug.Log("������ ���̾��ũ �Ҵ� ����");
                return;
            }

            // ������ ������
            if(!itemStack.isFull && item.GetAcquireType == Item.AcquireType.Stack)
            {
                itemStack.PushItem(item);
            }
            // �Ҹ��� ������
            else if(item.GetAcquireType == Item.AcquireType.Consume)
            {
                item.GetItem();
            }
        }
    }
}
