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
    [Tooltip("���� �� ����")]
    public int slotMaxRow;
    [Tooltip("���� �� ����")]
    public int slotMaxCol;
    [Tooltip("���� �� �е�")]
    public float rowPadding;
    [Tooltip("���� �� �е�")]
    public float colPadding;

    // �Ҵ��� ������ ũ��
    private Vector3? itemSize = null;
    // ���� ���� ������ǥ - (0,0)����
    private Vector3 firstSlotPosition;
    // ������ ����
    private Vector3 itemSpacing;
    // ���� ��
    private int slotCurRow = 0;
    // ���� ��
    private int slotCurCol = 0;

    private void Awake()
    {
        stack = new Stack<Item>();
    }

    private Vector3 GetItemPosition(Item item)
    {
        Vector3 itemLocalPos = Vector3.zero;
        // ���� �Ҵ� �� ������ ũ��, ���� ��ġ �Ҵ�
        if (itemSize == null)
        {
            itemSize = item.GetComponent<Renderer>().bounds.size;
            // �����۰� ���� �Ҵ�
            itemSpacing = new Vector3(itemSize.Value.x + rowPadding, 0, itemSize.Value.z + colPadding);

            // ���� ũ�� �Ҵ�
            float slotWidth = itemSize.Value.x * slotMaxRow + rowPadding * (slotMaxRow - 1);
            float slotHeight = itemSize.Value.z * slotMaxCol + colPadding * (slotMaxCol - 1);
            // (0,0) ���� ������ǥ �Ҵ�
            firstSlotPosition = new Vector3(slotWidth / 2f - itemSize.Value.x/2f, 0, -slotHeight / 2f + itemSize.Value.z / 2f);
        }
        
        // �� ó��
        if (slotCurRow == slotMaxRow)
        { 
            slotCurRow = 0;
            slotCurCol++;
        }

        // �������� �߰� �� ���� ��ġ �Ҵ�
        itemLocalPos =  new Vector3
            (firstSlotPosition.x - itemSpacing.x * slotCurRow + (slotCurRow == 0 ? 0 : rowPadding) 
            , 0
            , firstSlotPosition.z + itemSpacing.z * slotCurCol + (slotCurCol == 0 ? 0 : colPadding));
        
        slotCurRow++;
        return itemLocalPos;
    }

    public bool PushItem(Item item)
    {
        // ������ ���� �뷮�� �� �� ��� 
        if (slotMaxRow * slotMaxCol <= stack.Count)
            return false;

        // ������ ��ġ ����
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
