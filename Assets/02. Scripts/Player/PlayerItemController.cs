using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemController : ItemController
{
    private void OnTriggerEnter(Collider other)
    {
        // 테이블 상호작용
        if (LayerMaskMap.Instance.tableLM.Contain(other.gameObject.layer))
        {
            Table table = other.GetComponent<Table>();
            if (table == null)
            {
                Debug.Log($"테이블 레이어 등록오류 : {other.gameObject.name}");
                return;
            }
            table.InteractStack(itemStack);
        }
        // 소모형 아이템 습득
        else if (LayerMaskMap.Instance.itemLM.Contain(other.gameObject.layer))
        {
            Item item = other.GetComponent<Item>();

            if (item == null)
            {
                Debug.Log("아이템 레이어마스크 할당 오류");
                return;
            }

            // 소모형 아이템
            if (item.GetAcquireType == Item.AcquireType.Consume)
            {
                item.GetItem();
            }
        }
    }
}
