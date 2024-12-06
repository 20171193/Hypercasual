using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아이템 컨트롤러 (고객, 플레이어)
/// </summary>
public class ItemController : MonoBehaviour
{
    [SerializeField]
    private Stack<Item> itemStack;
    public Stack<Item> ItemStack { get { return itemStack; } }

    private void Awake()
    {
        itemStack = new Stack<Item>();
    }

    public void GetItem(Item item)
    {

    }

}
