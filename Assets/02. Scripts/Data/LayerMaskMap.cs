using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerMaskMap : Singleton<LayerMaskMap>
{
    [Header("플레이어")]
    public LayerMask playerLM;

    [Header("고객")]
    public LayerMask CustomerLM;

    [Header("아이템")]
    public LayerMask itemLM;

    [Header("테이블")]
    public LayerMask tableLM;
}
