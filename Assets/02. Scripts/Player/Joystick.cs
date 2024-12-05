using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Joystick : MonoBehaviour
{
    [Header("-Components")]
    [SerializeField]
    private Canvas mainCanvas;

    [Tooltip("���̽�ƽ ��ġ")]
    [SerializeField]    // ���̽�ƽ ���̽� (���)
    private RectTransform baseRT;
    // ���̽�ƽ ���̽� ������
    private float baseRadius;

    [Tooltip("��ƽ ��ġ")]
    [SerializeField]    // ��Ʈ�ѷ� (��ƽ)
    private RectTransform stickRT;

    [Tooltip("���콺 ������ ��ġ")]
    [SerializeField]    // ��ũ�� �� ���콺�� ��ġ (�巡��)
    public RectTransform pointerRT;
    private Camera mainCamera;

    [Header("-Specs")]
    [Tooltip("��ƽ ��������")]
    [SerializeField, Range(50, 100)]
    private float stickThreshold;

    [Tooltip("���� ��ƽ ����")]
    [SerializeField]
    private Vector3 stickDir;
    public Vector3 StickDir {get { return stickDir; }}

    private void Start()
    {
        mainCamera = Camera.main;   // ���� ī�޶� ĳ��
        baseRadius = baseRT.sizeDelta.x / 2.0f; // ���̽�ƽ ������ �Ҵ�
    }

    // ���̽�ƽ Ȱ��ȭ (���� ��ġ ����)
    public void EnableJoystick(Vector2 inputPos)
    {
        baseRT.position = inputPos / mainCanvas.scaleFactor;
    }
    public void DisableJoystick()
    {
        // ��ƽ ��ġ, ������ ��ġ ���󺹱�
        stickRT.position = pointerRT.position = baseRT.position;
    }

    // ���̽�ƽ UI ������Ʈ
    public void UpdateJoystick(Vector2 inputPos)
    {
        pointerRT.position = inputPos / mainCanvas.scaleFactor;

        // ���� ���� (������ - ��ƽ ���̽�)
        Vector3 inputVec = pointerRT.position - baseRT.position;
        
        // ��ƽ ���� �Ҵ�
        stickDir = inputVec.normalized;
        // ��ƽ UI ������Ʈ
        stickRT.localPosition = Vector3.ClampMagnitude(inputVec, stickThreshold);
    }
}
