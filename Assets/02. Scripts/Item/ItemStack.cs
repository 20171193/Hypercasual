using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStack : MonoBehaviour
{
    /// <summary>
    /// ������ ���� Ÿ��
    /// Horizontal = x/z
    /// Vertical = x/y
    /// </summary>
    public enum StackingType
    {
        Horizontal = 0,
        Vertical
    }

    // ���� ������ Ÿ��
    public enum StackOwner
    {
        None = -1,
        Player,
        Customer,
        Basket
    }

    [SerializeField]
    private Item itemTest;

    [SerializeField]
    private Stack<Item> stack;

    [Header("-Specs")]
    [Tooltip("������ ���� Ÿ�� x/z��� or x/y���")]
    public StackingType stackingType = StackingType.Horizontal;
    [Tooltip("���� ������")]
    [SerializeField]
    private StackOwner owner = StackOwner.None;
    public StackOwner Owner {get{return owner;}}    
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

    [ContextMenu("TestItem")]
    public void TestItem()
    {
        if (itemTest == null)
            return;
        Item inst = Instantiate(itemTest);

        PushItem(inst);
        Debug.Log(itemSize.Value);
    }

    // ���õ� ���� ��ǥ ��ȯ
    public Vector3 GetStackingWorldPosition(Item item)
    {
        Vector3 localPosition = GetStackingLocalPosition(item);
        return transform.position + localPosition;
    }
    private Vector3 GetStackingLocalPosition(Item item)
    {
        Vector3 itemLocalPos = Vector3.zero;
        // ���� �Ҵ� �� ������ ũ��, ���� ��ġ �Ҵ�
        if (itemSize == null)
        {
            itemSize = item.ItemRenderer.bounds.size;
            // �����۰� ���� �Ҵ�
            itemSpacing = new Vector3(itemSize.Value.x + rowPadding, 
                stackingType == StackingType.Vertical ? itemSize.Value.y + colPadding : 0,        // x/y ��� ���� ����ŷ
                stackingType == StackingType.Horizontal ? itemSize.Value.z + colPadding : 0);     // x/z ��� ���� ����ŷ
        }

        // �� ó��
        if (slotCurRow == slotMaxRow)
        {
            slotCurRow = 0;
            slotCurCol++;
        }

        // ������ ������ġ �Ҵ� (����)
        // �� ��/���� ù��° ��Ҵ� �е��� �߰����� ����.
        itemLocalPos = new Vector3(itemSpacing.x * slotCurRow + (slotCurRow == 0 ? 0 : rowPadding)
            , stackingType == StackingType.Vertical ? (itemSpacing.y * slotCurCol + (slotCurCol == 0 ? 0 : colPadding)) : 0
            , stackingType == StackingType.Horizontal ? (itemSpacing.z * slotCurCol + (slotCurCol == 0 ? 0 : colPadding)) : 0);

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
        item.transform.localPosition = GetStackingLocalPosition(item);
        item.transform.forward = transform.forward;
        stack.Push(item);
        return true;
    }
    public Item PopItem()
    {
        return stack.Pop();
    }
}
