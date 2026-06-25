using System.Collections;
using UnityEngine;

public class WarpGateTrigger_Open : MonoBehaviour
{
    [SerializeField] private SpriteRenderer gateRenderer; // 게이트 Sprite Renderer
    [SerializeField] private Sprite closedSprite; // 닫힌 이미지
    [SerializeField] private Sprite[] openingSprites; // 열리는 이미지 1~4
    [SerializeField] private float frameDelay = 0.08f; // 프레임 간격

    private Coroutine currentRoutine; // 현재 재생중인 코루틴

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) // Player가 아니면 무시
        {
            return;
        }

        PlayAnimation(openingSprites); // 열림 애니메이션
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) // Player가 아니면 무시
        {
            return;
        }

        Sprite[] closingSprites = new Sprite[openingSprites.Length]; // 닫힘 애니메이션(배열)

        for (int i = 0; i < openingSprites.Length; i++)
        {
            closingSprites[i] = openingSprites[openingSprites.Length - 1 - i]; // 역순으로 닫기
        }
        
        PlayAnimation(closingSprites, closedSprite); // 닫힘 애니메이션
    }


    private void PlayAnimation(Sprite[] sprites, Sprite finalSprite = null)
    {
        if (!gameObject.activeInHierarchy) // 오브젝트가 꺼진 상태면 코루틴 실행 안함
        {
            return;
        }

        if (currentRoutine != null)
        {
            StopCoroutine(currentRoutine); // 기존 애니메이션 중지
        }

        currentRoutine = StartCoroutine(PlayRoutine(sprites, finalSprite));
    }

    private IEnumerator PlayRoutine(Sprite[] sprites, Sprite finalSprite)
    {
        foreach (Sprite sprite in sprites)
        {
            gateRenderer.sprite = sprite; // 프레임 변경
            yield return new WaitForSeconds(frameDelay);
        }

        if (finalSprite != null)
        {
            gateRenderer.sprite = finalSprite; // 최종 이미지
        }
    }

    private void OnDisable()
    {
        if (currentRoutine != null)
        {
            StopCoroutine(currentRoutine); // 오브젝트 비활성화 시 코루틴 정지
            currentRoutine = null;
        }
    }
}
