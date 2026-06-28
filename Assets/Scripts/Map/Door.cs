using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private BoxCollider2D doorCollider; // 문 충돌체
    [SerializeField] private SpriteRenderer spriteRenderer; // 문 이미지

    [SerializeField] private Sprite closedSprite; // 닫힌 문 이미지
    [SerializeField] private Sprite[] openingSprites; // 열리는 이미지 5~9
    [SerializeField] private float frameDelay = 0.08f; // 프레임 간격

    [SerializeField] private bool startOpen; // 시작 시 문 열림 여부

    private Coroutine currentRoutine; // 현재 재생중인 코루틴
    private bool isOpen; // 문 열림 여부

    private void Reset()
    {
        doorCollider = GetComponent<BoxCollider2D>(); // Collider 자동 연결
        spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRenderer 자동 연결
    }
    private void Awake()
    {
        if (startOpen)
        {
            OpenInstant(); // 시작 시 열린 상태
        }

        else
        {
            CloseInstant(); // 시작 시 닫힌 상태
        }
    }

    private void OnValidate()
    {
        doorCollider = GetComponent<BoxCollider2D>(); // Inspector 변경 시 Collider 자동 연결
        spriteRenderer = GetComponent<SpriteRenderer>(); // Inspector 변경 시 SpriteRenderer 자동 연결
    }

    public void Open()
    {
        if (isOpen)
        {
            return; // 이미 열려있으면 무시
        }

        isOpen = true; // 문 열림

        if (doorCollider != null)
        {
            doorCollider.enabled = false; // 열리면 통과 가능
        }

        PlayAnimation(openingSprites); // 열림 애니메이션
    }

    public void Close()
    {
        if (!isOpen)
        {
            return; // 이미 닫혀있으면 무시
        }

        isOpen = false; // 문 닫힘

        if (doorCollider != null)
        {
            doorCollider.enabled = true; // 닫히면 통과 불가
        }

        Sprite[] closingSprites = new Sprite[openingSprites.Length]; // 닫힘 애니메이션

        for (int i = 0; i < openingSprites.Length; i++)
        {
            closingSprites[i] = openingSprites[openingSprites.Length - 1 - i]; // 역순으로 닫기
        }

        PlayAnimation(closingSprites, closedSprite); // 닫힘 애니메이션
    }

    private void OpenInstant()
    {
        isOpen = true; // 열린 상태

        if (doorCollider != null)
        {
            doorCollider.enabled = false; // 통과 가능
        }

        if (spriteRenderer != null && openingSprites != null && openingSprites.Length > 0)
        {
            spriteRenderer.sprite = openingSprites[openingSprites.Length - 1]; // 완전히 열린 이미지
            spriteRenderer.enabled = true;
        }
    }
    private void CloseInstant()
    {
        isOpen = false; // 닫힌 상태

        if (doorCollider != null)
        {
            doorCollider.enabled = true; // 시작 시 막힘
        }

        if (spriteRenderer != null && closedSprite != null)
        {
            spriteRenderer.sprite = closedSprite; // 닫힌 이미지 적용
            spriteRenderer.enabled = true; // 이미지 보이기
        }
    }

    private void PlayAnimation(Sprite[] sprites, Sprite finalSprite = null)
    {
        if (!isActiveAndEnabled)
        {
            return; // 오브젝트가 꺼져 있으면 실행 안 함
        }

        if (currentRoutine != null)
        {
            StopCoroutine(currentRoutine); // 기존 애니메이션 중지
        }

        currentRoutine = StartCoroutine(PlayRoutine(sprites, finalSprite));
    }

   private IEnumerator PlayRoutine(Sprite[] sprites, Sprite finalSprite)
    {
        if (sprites == null)
        {
            yield break; // 배열 없으면 종료
        }

        foreach (Sprite sprite in sprites)
        {
            if (spriteRenderer != null && sprite != null)
            {
                spriteRenderer.sprite = sprite; // 프레임 변경
            }

            yield return new WaitForSeconds(frameDelay);
        }

        if (spriteRenderer != null && finalSprite != null)
        {
            spriteRenderer.sprite = finalSprite; // 최종 이미지
        }

        currentRoutine = null; // 코루틴 종료
    }

    private void OnDisable()
    {
        if (currentRoutine != null)
        {
            StopCoroutine(currentRoutine); // 비활성화 시 코루틴 정지
            currentRoutine = null;
        }
    }
}
