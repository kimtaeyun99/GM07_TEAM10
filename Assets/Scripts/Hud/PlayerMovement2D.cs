using UnityEngine;

public class PlayerMovement2D : MonoBehaviour
{
    [Header("이동 설정")]
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    void Start()
    {
        // 2D 리지드바디 컴포넌트 가져오기
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 💡 1. 키보드 입력 받기 (A/D는 X축, W/S는 Y축)
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");

        // 💡 2. 만약 탑뷰(상하좌우 자유이동) 게임이라면 대각선 이동 속도가 빨라지지 않게 normalized 해줍니다.
        // 만약 횡스크롤(좌우 이동+점프) 게임이라면 yInput을 지우고 new Vector2(xInput, 0)만 사용하세요.
        moveInput = new Vector2(xInput, yInput).normalized;
    }

    void FixedUpdate()
    {
        // 💡 3. Rigidbody2D를 이용한 물리 이동 처리
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, moveInput.y * moveSpeed);

        // (보너스) 만약 사이드뷰 게임이라 중력이 켜져 있다면 위 코드를 아래처럼 바꿔야 중력을 받습니다:
        // rb.velocity = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);
    }
}