using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour // 플레이어의 움직임과 애니메이션을 담당하는 MonoBehaviour 클래스
{
    [SerializeField] private PlayerStats stats;

    private Animator animator;
    private Vector2 moveInput;
    private Rigidbody2D rb;
    private Vector2 last_direction = new Vector2(0, 0);
    private bool isDodge = false;
    private bool canDodge = true;


    public bool IsDodge()   // 회피중인지 외부에서 꺼내쓸수있게 빼놈
    {
        return isDodge;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

    }

    public void SetMoveInput(Vector2 input)
    {
        moveInput = input;
    }

    private void FixedUpdate()
    {
        if (isDodge) return; // 회피중이면 리턴

        Movement();

    }

    public void TryDodge()
    {
        Debug.Log("TryDodge");
        if (!canDodge || isDodge) return;   // 회피를 할수없거나 회피중일때 리턴

        StartCoroutine(DodgeRoutine());

    }

    private IEnumerator DodgeRoutine()
    {

        canDodge = false;
        isDodge = true;

        Debug.Log(last_direction);
        rb.linearVelocity = last_direction * stats.DodgeSpeed;  // 마지막 입력받은 방향으로 speed 만큼 이동

        yield return new WaitForSeconds(stats.DodgeDuration);  // 캐릭터의 회피시간만큼

        rb.linearVelocity = Vector2.zero;  // 끝나면 속도 제거

        isDodge = false;
        yield return new WaitForSeconds(stats.DodgeCooltime); // 캐릭터의 회피 쿨만큼

        canDodge = true;
    }


    private void Movement()
    {
        int ani_direction = -1;   // 애니메이션 상태는 기본 -1로 지정 (Idle 상태) 

        last_direction = moveInput;  // 마지막 방향 입력

        switch (moveInput.x)    // 수평값 case 구분하여 애니메이션 상태값 변수 변경
        {
            case -1:
                ani_direction = 3;
                break;
            case 1:
                ani_direction = 2;
                break;

        }
        switch (moveInput.y)    // 수직값 case 구분하여 애니메이션 상태값 변수 변경
        {
            case -1:
                ani_direction = 0;
                break;
            case 1:
                ani_direction = 1;
                break;

        }


        moveInput.Normalize(); // dir Vector의 길이를 1로 계산 (대각선 이동시 더 빨라지는걸 방지)
        animator.SetBool("IsMoving", moveInput.magnitude > 0);   // Bool 타입의 애니메이션 상태값 IsMoving 파라미터를 설정 (백터크기가 0보다크면 true) 
        animator.SetInteger("Direction", ani_direction); // 최종 애니메이션 상태값 변경

        rb.linearVelocity = stats.MoveSpeed * moveInput; // 최종 이동
    }


}

