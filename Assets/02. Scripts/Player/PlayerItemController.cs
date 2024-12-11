using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemController : ItemController
{
    private void OnTriggerEnter(Collider other)
    {
        // �÷��̾� ��ȣ�ۿ� ����
        if (LayerMaskMap.Instance.tableLM.Contain(other.gameObject.layer) 
            || LayerMaskMap.Instance.itemLM.Contain(other.gameObject.layer))
        {
            IPlayerInteractable interactable = other.GetComponent<IPlayerInteractable>();
            if(interactable != null)
            {
                interactable.EnterPlayer(this);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // �÷��̾� ��ȣ�ۿ� ����
        if (LayerMaskMap.Instance.tableLM.Contain(other.gameObject.layer)
            || LayerMaskMap.Instance.itemLM.Contain(other.gameObject.layer))
        {
            IPlayerInteractable interactable = other.GetComponent<IPlayerInteractable>();
            if (interactable != null)
            {
                interactable.ExitPlayer();
            }
        }
    }
}
