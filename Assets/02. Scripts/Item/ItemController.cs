using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아이템 컨트롤러 (고객, 플레이어)
/// </summary>
public class ItemController : MonoBehaviour
{
    [Header("-Components")]
    [Tooltip("아이템 스택")]
    [SerializeField]
    private ItemStack itemStack;

    private void OnTriggerEnter(Collider other)
    {
        // 아이템과 충돌 체크
        if(LayerMaskMap.Instance.itemLM.Contain(other.gameObject.layer))
        {
            Item item = other.GetComponent<Item>();

            if(item == null)
            {
                Debug.Log("아이템 레이어마스크 할당 오류");
                return;
            }

            // 스택형 아이템
            if(!itemStack.isFull && item.GetAcquireType == Item.AcquireType.Stack)
            {
                itemStack.PushItem(item);
            }
            // 소모형 아이템
            else if(item.GetAcquireType == Item.AcquireType.Consume)
            {
                item.GetItem();
            }
        }
    }
}
