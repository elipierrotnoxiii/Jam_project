using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Player Lives")]
    public int playerLives = 3;
    public TextMeshProUGUI livesText;
    public GameObject gameOverPanel;

    [Header("Score System")]
    public int score = 0;
    public TextMeshProUGUI scoreText;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateLivesText();
        UpdateScoreUI();
        gameOverPanel.SetActive(false);
    }

    // =======================
    //     LIFE MANAGEMENT
    // =======================
    public void LoseLife()
    {
        playerLives--;
        UpdateLivesText();

        if (playerLives <= 0)
        {
            GameOver();
        }
    }

    private void UpdateLivesText()
    {
        livesText.text = "Lives: " + playerLives;
    }

    private void GameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // Pause the game
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Resume the game
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        playerLives = 3;
        UpdateLivesText();
        gameOverPanel.SetActive(false);
    }

    // =======================
    //     SCORE MANAGEMENT
    // =======================

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
         scoreText.text = "Score: " + score;
    }
}
