using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public TextMeshProUGUI scoreText;
    public Image[] HpImages;
    public Image[] BoomImages;
    public GameObject gameOverSet;
    public GameMain gameMain;
    public GameObject UI;
    
    public Player player;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        
    }

    
    /*private void Update()
    {
        UpdateScore(GameManager.Instance.score);
        UpdateHPIcon();
        /*UpdateBoomIcon();#1#
        
    }*/

    

    /*private void UpdateHPIcon()
    {
        if (player.hp <= 3)
        {
            for (int i = 0; i < 3; i++)
            {
                HpImages[i].color = new Color(1, 1, 1, 0);
            }

            for (int i = 0; i < player.hp; i++)
            {
                HpImages[i].color = new Color(1, 1, 1, 1);
            }
        }
    }*/
    
    /*private void UpdateBoomIcon()
    {
        if (player.boomSlot <= 3)
        {
            for (int i = 0; i < 3; i++)
            {
                BoomImages[i].color = new Color(1, 1, 1, 0);
            }

            for (int i = 0; i < player.boomSlot; i++)
            {
                BoomImages[i].color = new Color(1, 1, 1, 1);
            }
        }
    }*/

    /*void OnEnable()
    {
        PlayerHealth.OnPlayerDead += ShowGameOver;
    }

    void OnDisable()
    {
        PlayerHealth.OnPlayerDead -= ShowGameOver;
    }*/

    void ShowGameOver()
    {
        gameOverSet.SetActive(true);
        UI.SetActive(false);
        Time.timeScale = 0f; // 게임 멈추고 싶으면
    }
    

    public void UpdateScore(int score)
    {
        scoreText.text = "Score: " + score;
    }
    
    // ⭐ 리트라이 버튼용 함수
    public void OnClickRetry()
    {
        Time.timeScale = 1f;
        gameMain.ResetGame();
    }
    
    public void ResetUI()
    {
        UI.SetActive(true);
        gameOverSet.SetActive(false);
    }
}
