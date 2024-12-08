using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
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
        // ���̺� ��ȣ�ۿ�
        if (LayerMaskMap.Instance.tableLM.Contain(other.gameObject.layer))
        {
            IStackInteractable table = other.GetComponent<IStackInteractable>();
            if(table == null)
            {
                Debug.Log($"���̺� ���̾� ��Ͽ��� : {other.gameObject.name}");
                return;
            }
            table.InteractStack(itemStack);
        }
        // �Ҹ��� ������ ����
        else if (LayerMaskMap.Instance.itemLM.Contain(other.gameObject.layer))
        {
            Item item = other.GetComponent<Item>();

            if (item == null)
            {
                Debug.Log("������ ���̾��ũ �Ҵ� ����");
                return;
            }

            // �Ҹ��� ������
            if (item.GetAcquireType == Item.AcquireType.Consume)
            {
                item.GetItem();
            }
        }
    }
}
