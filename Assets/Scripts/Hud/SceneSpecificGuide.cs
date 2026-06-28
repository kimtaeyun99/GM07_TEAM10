using UnityEngine;

public class SceneSpecificGuide : MonoBehaviour
{
    [SerializeField] private GameObject guidePanel;

    private void Start()
    {
        // 이 스크립트가 배치된 씬이 켜지면 무조건 가이드 패널을 활성화합니다.
        if (guidePanel != null)
        {
            guidePanel.SetActive(true);
        }
    }
}