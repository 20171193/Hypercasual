using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerMaskMap : Singleton<LayerMaskMap>
{
    [Header("�÷��̾�")]
    public LayerMask playerLM;

    [Header("��")]
    public LayerMask CustomerLM;

    [Header("������")]
    public LayerMask itemLM;

    [Header("���̺�")]
    public LayerMask tableLM;
}
