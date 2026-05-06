using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Ready,
        Playing,
        GameOver
    }

    public GameState currentState;
    public static GameManager Instance;

    public int score = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddScore(int value)
    {
        score += value;
        UIManager.Instance.UpdateScore(score);
    }
    
    
    public void Init()
    {
        currentState = GameState.Ready;
    }

    public void StartGame()
    {
        currentState = GameState.Playing;
        Debug.Log("게임 시작!");
    }

    public void GameOver()
    {
        currentState = GameState.GameOver;
        Debug.Log("게임 오버!");
    }

    void Update()
    {
        if (currentState != GameState.Playing) return;

        // 게임 진행 로직
        
    }
}
