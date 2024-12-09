using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemController : ItemController
{
    private void OnTriggerEnter(Collider other)
    {
        // 플레이어 상호작용 실행
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
        // 플레이어 상호작용 종료
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
