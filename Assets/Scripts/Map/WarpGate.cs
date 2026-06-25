using UnityEngine;
using UnityEngine.SceneManagement;

public class WarpGate : MonoBehaviour
{
    [SerializeField] private bool isActive = true; // 워프게이트 활성화 여부
    [SerializeField] private Collider2D warpTrigger; // 플레이어가 닿을 트리거 콜라이더

    public bool IsActive
    {
        get { return isActive; }
    }

    private void Awake()
    {
        ApplyState(); // 시작할 때 활성화 상태 적용
    }

    public void Activate()
    {
        isActive = true; // 워프게이트 활성화
        ApplyState();
    }

    public void Deactivate()
    {
        isActive = false; // 워프게이트 비활성화
        ApplyState();
    }

    private void ApplyState()
    {
        if (warpTrigger != null)
        {
            warpTrigger.enabled = isActive; // 활성 상태에 따라 트리거 켜기/끄기
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActive)
        {
            return; // 비활성화 상태면 작동하지 않음
        }

        if (!other.CompareTag("Player"))
        {
            return; // Player가 아니면 무시
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.ClearGame(); // 게임 클리어 상태
        }

        SceneManager.LoadScene("StartMenu"); // 시작 화면으로 이동
    }
}