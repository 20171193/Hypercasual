using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStack : MonoBehaviour
{
    [SerializeField]
    private Item itemTest;

    [SerializeField]
    private Stack<Item> stack;

    [Header("-Specs")]
    [Tooltip("슬롯 열 개수")]
    public int slotMaxRow;
    [Tooltip("슬롯 행 개수")]
    public int slotMaxCol;
    [Tooltip("슬롯 열 패딩")]
    public float rowPadding;
    [Tooltip("슬롯 행 패딩")]
    public float colPadding;

    // 할당한 아이템 크기
    private Vector3? itemSize = null;
    // 최초 슬롯 로컬좌표 - (0,0)슬롯
    private Vector3 firstSlotPosition;
    // 아이템 간격
    private Vector3 itemSpacing;
    // 현재 열
    private int slotCurRow = 0;
    // 현재 행
    private int slotCurCol = 0;

    private void Awake()
    {
        stack = new Stack<Item>();
    }

    private Vector3 GetItemPosition(Item item)
    {
        Vector3 itemLocalPos = Vector3.zero;
        // 최초 할당 시 아이템 크기, 슬롯 위치 할당
        if (itemSize == null)
        {
            itemSize = item.GetComponent<Renderer>().bounds.size;
            // 아이템간 간격 할당
            itemSpacing = new Vector3(itemSize.Value.x + rowPadding, 0, itemSize.Value.z + colPadding);

            // 슬롯 크기 할당
            float slotWidth = itemSize.Value.x * slotMaxRow + rowPadding * (slotMaxRow - 1);
            float slotHeight = itemSize.Value.z * slotMaxCol + colPadding * (slotMaxCol - 1);
            // (0,0) 슬롯 로컬좌표 할당
            firstSlotPosition = new Vector3(slotWidth / 2f - itemSize.Value.x/2f, 0, -slotHeight / 2f + itemSize.Value.z / 2f);
        }
        
        // 행 처리
        if (slotCurRow == slotMaxRow)
        { 
            slotCurRow = 0;
            slotCurCol++;
        }

        // 아이템을 추가 할 로컬 위치 할당
        itemLocalPos =  new Vector3
            (firstSlotPosition.x - itemSpacing.x * slotCurRow + (slotCurRow == 0 ? 0 : rowPadding) 
            , 0
            , firstSlotPosition.z + itemSpacing.z * slotCurCol + (slotCurCol == 0 ? 0 : colPadding));
        
        slotCurRow++;
        return itemLocalPos;
    }

    public bool PushItem(Item item)
    {
        // 아이템 스택 용량이 다 찬 경우 
        if (slotMaxRow * slotMaxCol <= stack.Count)
            return false;

        // 아이템 위치 설정
        item.transform.parent = transform;
        item.transform.localPosition = GetItemPosition(item);
        stack.Push(item);
        return true;
    }

    public Item PopItem()
    {
        return stack.Pop();
    }
}
