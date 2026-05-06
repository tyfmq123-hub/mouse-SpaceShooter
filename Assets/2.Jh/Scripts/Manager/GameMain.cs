using UnityEngine;

public class GameMain : MonoBehaviour
{
    public Transform playerTransform;
    public Transform responTransform;
    public Player player;
    public SpawnManager spawnManager;

    void Awake()
    {
        Application.targetFrameRate = 60;
    }

    public void ResetGame()
    {
        Time.timeScale = 1f;

        GameManager.Instance.score = 0;
        GameManager.Instance.StartGame();

        player.ResetPlayer();

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            enemy.SetActive(false);

        BulletStraightToPlayer[] bullets = Object.FindObjectsByType<BulletStraightToPlayer>(FindObjectsSortMode.None);
        foreach (BulletStraightToPlayer bullet in bullets)
            bullet.gameObject.SetActive(false);

        spawnManager.ResetStage();

        UIManager.Instance.ResetUI();
        UIManager.Instance.UpdateScore(0);
    }
}
