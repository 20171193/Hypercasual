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

    [SerializeField]    // Ŭ�� �׼�
    private InputAction clickAction;
    [SerializeField]    // �巡�� �׼�
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
            // �ִϸ����� ������Ʈ
            anim.SetBool(paramID_IsMoving, isMoving);
        } 
    }

    [SerializeField]
    private Vector2 startDragPos;
    [SerializeField]
    private Vector2 curDragPos;
    [SerializeField]
    private Vector2 inputDir;

    // �ִϸ����� �Ķ���� ID
    private int paramID_IsMoving = -1;

    private void Awake()
    {
        paramID_IsMoving = Animator.StringToHash("IsMoving");
    }

    private void OnEnable()
    {
        // Input Action Ȱ��ȭ, ���ε�
        clickAction.Enable();
        clickAction.performed += OnBeginDrag;
        clickAction.canceled += OnEndDrag;
        // �巡�׾׼��� ���ε��� ���� 
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

    // Input System �ݹ�
    private void OnBeginDrag(InputAction.CallbackContext context)
    {
        // �巡�� Ȱ��ȭ
        dragAction.Enable();
        // ���� ���콺 ��ġ �Ҵ�
        startDragPos = Input.mousePosition;

        // ���̽�ƽ Ȱ��ȭ
        joystick.gameObject.SetActive(true);
        joystick.EnableJoystick(startDragPos);
        IsMoving = true;
    }
    private void OnEndDrag(InputAction.CallbackContext context)
    {
        IsMoving = false;

        // �巡�� ��Ȱ��ȭ 
        dragAction.Disable();

        // ���̽�ƽ ��Ȱ��ȭ
        joystick.DisableJoystick();
        joystick.gameObject.SetActive(false);
    }
    private void OnDrag(InputAction.CallbackContext context)
    {
        // ���� ���콺 ��ǥ �Ҵ�
        curDragPos = context.ReadValue<Vector2>();
        // ���̽�ƽ UI ������Ʈ
        joystick.UpdateJoystick(curDragPos);
    }
    
    // ���̽�ƽ �̵�
    private void Move()
    {
        // ���̽�ƽ ���⺤�� ��ȯ
        Vector3 moveDir = new Vector3(joystick.StickDir.x, 0, joystick.StickDir.y);
        // �÷��̾� ȸ��
        transform.forward = -moveDir;
        controller.Move(transform.forward * moveSpeed * Time.deltaTime);
        Debug.Log(controller.velocity.magnitude);

        // �ִϸ����� ����
        anim.SetBool(paramID_IsMoving, true);
    }
}
