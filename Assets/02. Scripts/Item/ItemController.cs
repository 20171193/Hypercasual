using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

/// <summary>
/// ������ ��Ʈ�ѷ�
/// </summary>
public class ItemController : MonoBehaviour
{
    [Header("-Components")]
    [Tooltip("������ ����")]
    [SerializeField]
    protected ItemStack itemStack;
    public ItemStack ItemStack {get { return itemStack; } }
}
