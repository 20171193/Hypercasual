using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

/// <summary>
/// 빵 저장소 테이블
/// </summary>
public class Basket : MonoBehaviour
{
    [SerializeField]
    private LayerMask customerLM;
    [SerializeField]
    private LayerMask playerLM;

    [SerializeField]
    private ItemStack itemStack;

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
