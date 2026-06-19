using UnityEngine;
using UnityEngine.InputSystem;

public class TestPlayerMove : MonoBehaviour //테스트용 상하좌우 이동 플레이어.
{
    [SerializeField] private float moveSpeed = 5f; //테스트 플레이어 이동 속도

    private Rigidbody2D rb; //플레이어 이동에 사용할 RigidBody2d
    private Vector2 moveInput; //키보드 입력값 저장
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        moveInput = Vector2.zero; // 매 프레임마다 입력값 초기화

        if (Keyboard.current == null) { return; } // 키보드가 감지되지 않으면 실행 중단
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) 
        { 
            moveInput.x -= 1f; // 왼쪽 이동
        }
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
        {
            moveInput.x += 1f; // 오른쪽 이동
        }
        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
        {
            moveInput.y += 1f; // 위쪽 이동
        }
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
        {
            moveInput.y -= 1f; // 아래쪽 이동
        }

        moveInput = moveInput.normalized; //대각선 이동 빨라지는걸 방지
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed; //입력 방향으로 이동
    }
}
