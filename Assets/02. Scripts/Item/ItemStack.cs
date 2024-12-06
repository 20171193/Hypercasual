using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아이템 스택 타입
/// Horizontal = x/z
/// Vertical = x/y
/// </summary>
public enum StackType
{
    Horizontal = 0,
    Vertical
}

public class ItemStack : MonoBehaviour
{
    [SerializeField]
    private Item itemTest;

    [SerializeField]
    private Stack<Item> stack;

    [Header("-Specs")]
    [Tooltip("아이템 스택 타입 x/z평면 or x/y평면")]
    public StackType stackType = StackType.Horizontal;
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

    [ContextMenu("TestItem")]
    public void TestItem()
    {
        if (itemTest == null)
            return;
        Item inst = Instantiate(itemTest);

        PushItem(inst);
        Debug.Log(itemSize.Value);
    }

    private Vector3 GetItemPosition(Item item)
    {
        Vector3 itemLocalPos = Vector3.zero;
        // 최초 할당 시 아이템 크기, 슬롯 위치 할당
        if (itemSize == null)
        {
            itemSize = item.ItemRenderer.bounds.size;
            // 아이템간 간격 할당
            itemSpacing = new Vector3(itemSize.Value.x + rowPadding, 
                stackType == StackType.Vertical ? itemSize.Value.y + colPadding : 0,        // x/y 평면 기준 스태킹
                stackType == StackType.Horizontal ? itemSize.Value.z + colPadding : 0);     // x/z 평면 기준 스태킹
        }

        // 행 처리
        if (slotCurRow == slotMaxRow)
        {
            slotCurRow = 0;
            slotCurCol++;
        }

        // 아이템 슬롯위치 할당 (로컬)
        // 각 행/열의 첫번째 요소는 패딩을 추가하지 않음.
        itemLocalPos = new Vector3(itemSpacing.x * slotCurRow + (slotCurRow == 0 ? 0 : rowPadding)
            , stackType == StackType.Vertical ? (itemSpacing.y * slotCurCol + (slotCurCol == 0 ? 0 : colPadding)) : 0
            , stackType == StackType.Horizontal ? (itemSpacing.z * slotCurCol + (slotCurCol == 0 ? 0 : colPadding)) : 0);

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
        item.transform.forward = transform.forward;
        stack.Push(item);
        return true;
    }
    public Item PopItem()
    {
        return stack.Pop();
    }
}
