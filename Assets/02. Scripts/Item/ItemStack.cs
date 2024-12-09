using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Progress;

public class ItemStack : MonoBehaviour
{
    /// <summary>
    /// 스택 소유자 타입
    /// </summary>
    public enum StackOwner 
    {
        Player = 0,
        Customer,
        Table
    }
    /// <summary>
    /// Horizontal : x/z 평면 스택 | 
    /// Vertical : x/y 평면 스택
    /// </summary>
    public enum StackStyle
    {
        Horizontal = 0,
        Vertical
    }

    [Tooltip("아이템 스택 완료")]
    public UnityEvent OnStackedItem; 

    [SerializeField]
    private Item itemTest;

    [Space(10)]
    [Header("-Specs")]
    [Tooltip("아이템 스택 타입 x/z평면 or x/y평면")]
    [SerializeField]
    private StackStyle style;
    public StackStyle Style { get { return style; } }
    [Tooltip("스택 소유자")]
    [SerializeField]
    private StackOwner owner;
    public StackOwner Onwer {get { return owner; } }
    [Tooltip("최대 스택 개수")]
    [SerializeField]
    private int maxStackCount;
    [Tooltip("슬롯 열 개수")]
    public int slotMaxRow;
    [Tooltip("슬롯 행 개수")]
    public int slotMaxCol;
    [Tooltip("슬롯 열 패딩")]
    public float rowPadding;
    [Tooltip("슬롯 행 패딩")]
    public float colPadding;
    [Tooltip("아이템 스태킹 속도")]
    public float stackingSpeed;

    [Space(10)]
    [Header("-Ballancing")]
    [SerializeField]
    private int curStackCount;
    public virtual int CurStackCount
    {
        set 
        {
            curStackCount = value;
            if (curStackCount >= slotMaxRow * slotMaxCol)
                isFull = true;
            else
                isFull = false;
        }
        get { return curStackCount; }
    }

    public bool isFull;
    private Stack<Item> stack;
    // 할당한 아이템 크기
    private Vector3? itemSize = null;
    // 아이템 간격
    private Vector3 itemSpacing;
    // 현재 열
    private int slotCurRow = 0;
    // 현재 행
    private int slotCurCol = 0;

    // 아이템 스택 루틴 : 베지어 곡선
    protected Coroutine stackingRoutine;

    protected virtual void Awake()
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

    // 스택될 월드 좌표 반환
    public Vector3 GetStackingWorldPosition(Item item)
    {
        Vector3 localPosition = CalculateLocalPosition(item);
        return transform.position + localPosition;
    }
    protected Vector3 CalculateLocalPosition(Item item)
    {
        // 최초 할당 시 아이템 크기, 슬롯 위치 할당
        if (itemSize == null)
        {
            itemSize = item.ItemRenderer.bounds.size;
            // 아이템간 간격 할당
            itemSpacing = new Vector3(itemSize.Value.x / 2f + rowPadding,
                style == StackStyle.Vertical ? itemSize.Value.y / 2f + colPadding : 0,        // x/y 평면 기준 스태킹
                style == StackStyle.Horizontal ? itemSize.Value.z / 2f + colPadding : 0);     // x/z 평면 기준 스태킹
        }

        // 아이템 슬롯위치 할당 (로컬)
        // 각 행/열의 첫번째 요소는 패딩을 추가하지 않음.
        Vector3 itemLocalPos = new Vector3(itemSpacing.x * slotCurRow + (slotCurRow == 0 ? 0 : rowPadding)
            , style == StackStyle.Vertical ? (itemSpacing.y * slotCurCol + (slotCurCol == 0 ? 0 : colPadding)) : 0
            , style == StackStyle.Horizontal ? (itemSpacing.z * slotCurCol + (slotCurCol == 0 ? 0 : colPadding)) : 0);

        return itemLocalPos;
    }
    public virtual bool PushItem(Item item)
    {
        // 아이템 스택 용량이 다 찬 경우 
        if (isFull)
            return false;

        Vector3 origin = item.transform.position;
        Vector3 destination = GetStackingWorldPosition(item);
        // 아이템 스태킹 실행
        item.GetItem();
        CurStackCount++;
        stackingRoutine = StartCoroutine(StackingRoutine(item, origin, destination));
        return true;
    }
    public virtual Item PopItem()
    {
        CurStackCount--;
        // 스택 슬롯 인덱스처리
        slotCurRow--;
        if(slotCurRow < 0)
        {
            slotCurRow = slotMaxRow-1;
            slotCurCol--;
        }
        return stack.Pop();
    }
    protected virtual IEnumerator StackingRoutine(Item item, Vector3 origin, Vector3 destination)
    {
        // 경유 지점 할당
        Vector3 midPos = Vector3.Lerp(origin, destination, 0.5f) + Vector3.up * 5f;

        // 베지어 곡선
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
        // 루틴이후 목적지 재설정(루틴 간 위치 변경고려)
        item.transform.localPosition = CalculateLocalPosition(item);
        stack.Push(item);

        // 슬롯 인덱스처리
        slotCurRow++;
        if (slotCurRow == slotMaxRow)
        {
            slotCurRow = 0;
            slotCurCol++;
        }
        
        OnStackedItem?.Invoke();
        yield return null;
    }
}
