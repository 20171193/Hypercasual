using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("-Components")]
    [SerializeField]
    private CharacterController controller;
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private Joystick joystick;

    [SerializeField]    // 클릭 액션
    private InputAction clickAction;
    [SerializeField]    // 드래그 액션
    private InputAction dragAction;

    [Space(10)]
    [Header("-Specs")]
    [SerializeField]
    private float moveSpeed;

    // Movement
    private bool isMoving = false;
    private bool IsMoving 
    {
        set 
        {
            isMoving = value;
            // 애니메이터 업데이트
            anim.SetBool(paramID_IsMoving, isMoving);
        } 
    }

    [SerializeField]
    private Vector2 startDragPos;
    [SerializeField]
    private Vector2 curDragPos;
    [SerializeField]
    private Vector2 inputDir;

    // 애니메이터 파라미터 ID
    private int paramID_IsMoving = -1;

    private void Awake()
    {
        paramID_IsMoving = Animator.StringToHash("IsMoving");
    }

    private void OnEnable()
    {
        // Input Action 활성화, 바인딩
        clickAction.Enable();
        clickAction.performed += OnBeginDrag;
        clickAction.canceled += OnEndDrag;
        // 드래그액션은 바인딩만 진행 
        dragAction.performed += OnDrag;
    }
    private void OnDisable()
    {
        clickAction.performed -= OnBeginDrag;
        clickAction.canceled -= OnEndDrag;
        clickAction.Disable();

        dragAction.performed -= OnDrag;
    }

    private void Update()
    {
        if (isMoving)
            Move();
    }

    // Input System 콜백
    private void OnBeginDrag(InputAction.CallbackContext context)
    {
        // 드래그 활성화
        dragAction.Enable();
        // 최초 마우스 위치 할당
        startDragPos = Input.mousePosition;

        // 조이스틱 활성화
        joystick.gameObject.SetActive(true);
        joystick.EnableJoystick(startDragPos);
        IsMoving = true;
    }
    private void OnEndDrag(InputAction.CallbackContext context)
    {
        IsMoving = false;

        // 드래그 비활성화 
        dragAction.Disable();

        // 조이스틱 비활성화
        joystick.DisableJoystick();
        joystick.gameObject.SetActive(false);
    }
    private void OnDrag(InputAction.CallbackContext context)
    {
        // 현재 마우스 좌표 할당
        curDragPos = context.ReadValue<Vector2>();
        // 조이스틱 UI 업데이트
        joystick.UpdateJoystick(curDragPos);
    }
    
    // 조이스틱 이동
    private void Move()
    {
        // 조이스틱 방향벡터 변환
        Vector3 moveDir = new Vector3(joystick.StickDir.x, 0, joystick.StickDir.y);
        // 플레이어 회전
        transform.forward = -moveDir;
        controller.Move(transform.forward * moveSpeed * Time.deltaTime);
        Debug.Log(controller.velocity.magnitude);

        // 애니메이터 세팅
        anim.SetBool(paramID_IsMoving, true);
    }
}
