using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

/// <summary>
/// 아이템 컨트롤러
/// </summary>
public class ItemController : MonoBehaviour
{
    [Header("-Components")]
    [Tooltip("아이템 스택")]
    [SerializeField]
    protected ItemStack itemStack;
    public ItemStack ItemStack {get { return itemStack; } }
}
