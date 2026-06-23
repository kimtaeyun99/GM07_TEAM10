using UnityEngine;
using TMPro;
using System.Collections;

public class HUDLogManager : MonoBehaviour
{
    // 어디서나 접근 가능한 싱글톤
    public static HUDLogManager instance;

    [Header("UI 연결")]
    public Transform logContainer;    // 1단계에서 만든 TextLogContainer 연결
    public GameObject logTextPrefab; // 1단계에서 만든 LogTextItem 프리팹 연결

    [Header("설정")]
    public float logDisplayTime = 3f; // 로그가 유지되는 시간

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void Log(string message, Color textColor)
    {
        if (logContainer == null || logTextPrefab == null) return;

        // 💡 안전장치: 현재 컨테이너 하위의 자식(로그) 개수가 10개 이상이면 가장 오래된 첫 번째 로그 삭제
        if (logContainer.childCount >= 10)
        {
            // 0번째 자식이 가장 먼저 생성된 로그입니다.
            Destroy(logContainer.GetChild(0).gameObject);
        }

        // 프리팹을 컨테이너 하위에 생성
        GameObject newLog = Instantiate(logTextPrefab, logContainer);

        // 텍스트 컴포넌트를 가져와서 내용과 색상 변경
        TextMeshProUGUI tmp = newLog.GetComponent<TextMeshProUGUI>();
        if (tmp != null)
        {
            tmp.text = message;
            tmp.color = textColor;
        }

        // 일정 시간 후에 파괴하는 코루틴 시작
        StartCoroutine(FadeAndDestroyLog(newLog));
    }

    private IEnumerator FadeAndDestroyLog(GameObject logObject)
    {
        // 지정된 시간(예: 3초) 동안 대기
        yield return new WaitForSeconds(logDisplayTime);

        // 💡 핵심 안전장치: 대기하는 와중에 최대 개수 제한으로 인해 
        // 이미 오브젝트가 강제로 Destroy(삭제) 되었다면 코루틴을 즉시 종료합니다.
        if (logObject == null)
        {
            yield break;
        }

        // 오브젝트가 안전하게 살아있을 때만 아래 로직 실행
        TextMeshProUGUI tmp = logObject.GetComponent<TextMeshProUGUI>();
        if (tmp != null)
        {
            float duration = 0.5f; // 페이드아웃 지속 시간
            float currentTime = 0f;
            Color startColor = tmp.color;

            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;

                // 💡 혹시라도 반복문(Lerp) 도중에 오브젝트가 파괴될 경우를 대비한 2차 안전장치
                if (logObject == null) yield break;

                float alpha = Mathf.Lerp(1f, 0f, currentTime / duration);
                tmp.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
                yield return null;
            }
        }

        // 최종적으로 파괴할 때도 한 번 더 검사 후 안전하게 삭제
        if (logObject != null)
        {
            Destroy(logObject);
        }
    }
}