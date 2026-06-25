using UnityEngine;

public enum GameState
{
    Title, // 타이틀 화면
    Playing, // 게임 진행
    Clear, // 게임 클리어
    GameOver // 게임 오버
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // 싱글톤

    public GameState CurrentState { get; private set; } // 현재 상태

    private void Awake()
    {
        if (Instance != null && Instance != this) // 중복 방지
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // 씬 이동해도 유지
    }

    private void Start()
    {
        ChangeState(GameState.Title); // 시작 상태
    }

    public void ChangeState(GameState newState)
    {
        CurrentState = newState; // 상태 변경
        Debug.Log("Game State: " + newState);
    }

    public void StartGame()
    {
        ChangeState(GameState.Playing); // 게임 진행
    }

    public void ClearGame()
    {
        ChangeState(GameState.Clear); // 게임 클리어
    }

    public void GameOver()
    {
        ChangeState(GameState.GameOver); // 게임 오버
    }

}
