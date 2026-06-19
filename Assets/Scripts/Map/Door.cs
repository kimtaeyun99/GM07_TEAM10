using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Collider2D doorCollider; // 문 충돌체
    [SerializeField] private SpriteRenderer spriteRenderer; // 문 이미지
    [SerializeField] private bool isOpen; // 문 열림 여부

    private void Reset()
    {
        doorCollider = GetComponent<Collider2D>(); // Collider 자동 연결
        spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRenderer 자동 연결
    }
    private void Awake()
    {
        ApplyState(); // 게임 시작 시 문 상태 적용
    }

    private void OnValidate()
    {
        doorCollider = GetComponent<Collider2D>(); // Inspector 변경 시 Collider 자동 연결
        spriteRenderer = GetComponent<SpriteRenderer>(); // Inspector 변경 시 SpriteRenderer 자동 연결
    }

    public void Open() // 문 상태 열림
    {
        isOpen = true;
        ApplyState();
    }

    public void Close() // 문 상태 닫힘
    {
        isOpen = false;
        ApplyState();
    }

    private void ApplyState()
    {
        if (doorCollider != null)
        {
            doorCollider.enabled = !isOpen; // 열리면 충돌 off, 닫히면 충돌 on
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = !isOpen; // 열리면 이미지 숨기고, 닫히면 보이기
        }
    }
}
