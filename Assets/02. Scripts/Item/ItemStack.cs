using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Progress;

public class ItemStack : MonoBehaviour
{
    /// <summary>
    /// ���� ������ Ÿ��
    /// </summary>
    public enum StackOwner 
    {
        Player = 0,
        Customer,
        Table
    }
    /// <summary>
    /// Horizontal : x/z ��� ���� | 
    /// Vertical : x/y ��� ����
    /// </summary>
    public enum StackStyle
    {
        Horizontal = 0,
        Vertical
    }

    [Tooltip("������ ���� �Ϸ�")]
    public UnityEvent<Item> OnStackedItem;

    [SerializeField]
    private Item itemTest;

    [Header("-Specs")]
    [Tooltip("������ ���� Ÿ�� x/z��� or x/y���")]
    public StackStyle stackStyle = StackStyle.Horizontal;
    [Tooltip("�ִ� ���� ����")]
    [SerializeField]
    private int maxStackCount;
    [Tooltip("���� �� ����")]
    public int slotMaxRow;
    [Tooltip("���� �� ����")]
    public int slotMaxCol;
    [Tooltip("���� �� �е�")]
    public float rowPadding;
    [Tooltip("���� �� �е�")]
    public float colPadding;
    [Tooltip("������ ����ŷ �ӵ�")]
    public float stackingSpeed;

    [Header("-Ballancing")]
    [SerializeField]
    private Stack<Item> stack;

    private int curStackCount;
    public int CurStackCount
    {
        set 
        {
            curStackCount = value;
            if (curStackCount == slotMaxRow * slotMaxCol)
                isFull = true;
            else
                isFull = false;
        }
        get { return curStackCount; }
    }

    public bool isFull;

    // �Ҵ��� ������ ũ��
    private Vector3? itemSize = null;
    // ������ ����
    private Vector3 itemSpacing;
    // ���� ��
    private int slotCurRow = 0;
    // ���� ��
    private int slotCurCol = 0;

    // ������ ���� ��ƾ : ������ �
    protected Coroutine stackingRoutine;

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
        Vector3 localPosition = CalculateLocalPosition(item);
        return transform.position + localPosition;
    }
    protected Vector3 CalculateLocalPosition(Item item)
    {
        Vector3 itemLocalPos = Vector3.zero;
        // ���� �Ҵ� �� ������ ũ��, ���� ��ġ �Ҵ�
        if (itemSize == null)
        {
            itemSize = item.ItemRenderer.bounds.size;
            // �����۰� ���� �Ҵ�
            itemSpacing = new Vector3(itemSize.Value.x + rowPadding,
                stackStyle == StackStyle.Vertical ? itemSize.Value.y + colPadding : 0,        // x/y ��� ���� ����ŷ
                stackStyle == StackStyle.Horizontal ? itemSize.Value.z + colPadding : 0);     // x/z ��� ���� ����ŷ
        }

        // ������ ������ġ �Ҵ� (����)
        // �� ��/���� ù��° ��Ҵ� �е��� �߰����� ����.
        itemLocalPos = new Vector3(itemSpacing.x * slotCurRow + (slotCurRow == 0 ? 0 : rowPadding)
            , stackStyle == StackStyle.Vertical ? (itemSpacing.y * slotCurCol + (slotCurCol == 0 ? 0 : colPadding)) : 0
            , stackStyle == StackStyle.Horizontal ? (itemSpacing.z * slotCurCol + (slotCurCol == 0 ? 0 : colPadding)) : 0);

        return itemLocalPos;
    }
    public bool PushItem(Item item)
    {
        // ������ ���� �뷮�� �� �� ��� 
        if (slotMaxRow * slotMaxCol <= stack.Count)
            return false;

        Vector3 origin = item.transform.position;
        Vector3 destination = GetStackingWorldPosition(item);
        // ������ ����ŷ ����
        item.GetItem();
        stackingRoutine = StartCoroutine(StackingRoutine(item, origin, destination));
        return true;
    }
    public Item PopItem()
    {
        CurStackCount--;
        return stack.Pop();
    }
    protected virtual IEnumerator StackingRoutine(Item item, Vector3 origin, Vector3 destination)
    {
        // ���� ���� �Ҵ�
        Vector3 midPos = Vector3.Lerp(origin, destination, 0.5f) + Vector3.up * 5f;

        // ������ �
        float rate = 0f;
        while (rate < 1f)
        {
            Vector3 p1 = Vector3.Lerp(origin, midPos, rate);
            Vector3 p2 = Vector3.Lerp(midPos, destination, rate);
            item.transform.position = Vector3.Lerp(p1, p2, rate);
            rate += Time.deltaTime * stackingSpeed;
            yield return null;
        }

        item.transform.parent = transform;
        item.transform.forward = transform.forward;
        // ��ƾ���� ������ �缳��(��ƾ �� ��ġ ������)
        item.transform.localPosition = CalculateLocalPosition(item);
        stack.Push(item);

        // ���� �ε���ó��
        slotCurRow++;
        if (slotCurRow == slotMaxRow)
        {
            slotCurRow = 0;
            slotCurCol++;
        }
        CurStackCount++;
        OnStackedItem?.Invoke(item);
        yield return null;
    }
}
