using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
            Instance = this;
        else
            Destroy(gameObject);
    }

    void OnEnable()
    {
        Player.OnPlayerDead += ShowGameOver;
    }

    void OnDisable()
    {
        Player.OnPlayerDead -= ShowGameOver;
    }

    private void Update()
    {
        UpdateScore(GameManager.Instance.score);
        UpdateHPIcon();
        UpdateBoomIcon();
    }

    private void UpdateHPIcon()
    {
        for (int i = 0; i < HpImages.Length; i++)
            HpImages[i].color = new Color(1, 1, 1, 0);

        for (int i = 0; i < player.lives && i < HpImages.Length; i++)
            HpImages[i].color = new Color(1, 1, 1, 1);
    }

    private void UpdateBoomIcon()
    {
        for (int i = 0; i < BoomImages.Length; i++)
            BoomImages[i].color = new Color(1, 1, 1, 0);

        for (int i = 0; i < player.boomCount && i < BoomImages.Length; i++)
            BoomImages[i].color = new Color(1, 1, 1, 1);
    }

    void ShowGameOver()
    {
        gameOverSet.SetActive(true);
        UI.SetActive(false);
        Time.timeScale = 0f;
    }

    public void UpdateScore(int score)
    {
        scoreText.text = "Score: " + score;
    }

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
