using UnityEngine;

public class GameMain : MonoBehaviour
{
    
    
    public Transform playerTransform;
    public Transform responTransform;
    void Awake()
    {
        Application.targetFrameRate = 60;
    }

    
    
    
    public void ResetGame()
    {
        Time.timeScale = 1f;

        // 플레이어 초기화
        
        playerTransform.position = responTransform.position;

        // UI 초기화
        UIManager.Instance.ResetUI();
    }
}
