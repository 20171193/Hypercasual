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

    [Tooltip("조이스틱 위치")]
    [SerializeField]    // 조이스틱 베이스 (배경)
    private RectTransform baseRT;
    // 조이스틱 베이스 반지름
    private float baseRadius;

    [Tooltip("스틱 위치")]
    [SerializeField]    // 컨트롤러 (스틱)
    private RectTransform stickRT;

    [Tooltip("마우스 포인터 위치")]
    [SerializeField]    // 스크린 상 마우스의 위치 (드래그)
    public RectTransform pointerRT;
    private Camera mainCamera;

    [Header("-Specs")]
    [Tooltip("스틱 범위제한")]
    [SerializeField, Range(50, 100)]
    private float stickThreshold;

    [Tooltip("현재 스틱 방향")]
    [SerializeField]
    private Vector3 stickDir;
    public Vector3 StickDir {get { return stickDir; }}

    private void Start()
    {
        mainCamera = Camera.main;   // 메인 카메라 캐싱
        baseRadius = baseRT.sizeDelta.x / 2.0f; // 조이스틱 반지름 할당
    }

    // 조이스틱 활성화 (최초 위치 세팅)
    public void EnableJoystick(Vector2 inputPos)
    {
        baseRT.position = inputPos / mainCanvas.scaleFactor;
    }
    public void DisableJoystick()
    {
        // 스틱 위치, 포인터 위치 원상복구
        stickRT.position = pointerRT.position = baseRT.position;
    }

    // 조이스틱 UI 업데이트
    public void UpdateJoystick(Vector2 inputPos)
    {
        pointerRT.position = inputPos / mainCanvas.scaleFactor;

        // 벡터 도출 (포인터 - 스틱 베이스)
        Vector3 inputVec = pointerRT.position - baseRT.position;
        
        // 스틱 방향 할당
        stickDir = inputVec.normalized;
        // 스틱 UI 업데이트
        stickRT.localPosition = Vector3.ClampMagnitude(inputVec, stickThreshold);
    }
}
