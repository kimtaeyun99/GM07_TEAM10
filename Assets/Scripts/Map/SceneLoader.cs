using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadTutorialScene()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartGame(); // 게임 진행 상태
        }

        SceneManager.LoadScene("Stage_Tutorial"); // 튜토리얼 씬
    }

    public void LoadStartScene()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ChangeState(GameState.Title); // 시작 상태
        }

        SceneManager.LoadScene("StartMenu"); // 시작 화면
    }

    public void QuitGame()
    {
        Application.Quit(); // 게임 종료
    }
}
