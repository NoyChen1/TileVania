using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameSession : MonoBehaviour
{
    static GameSession instance;
    HighScoreManager highScoreManager = new HighScoreManager();

    [SerializeField] int playerLives = 3;
    [SerializeField] int playerScore = 0;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] GameObject[] lives;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] TextMeshProUGUI highScore;


    float delayTime = 1f;

    public static GameSession Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameSession>();
            }
            return instance;
        }
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    void Start()
    {
        UpdateUI();
        gameOverPanel.SetActive(false);
    }
    public void ProcessPlayerDeath()
    {
        if (playerLives > 1)
        {
            DecreaseLife();
        }
        else
        {
            EndGame();
        }
    }

    public void EndGame()
    {
        TriggerGameOverPanel();
        SaveHighScore();
        DesplayHighScore();
    }

    void SaveHighScore()
    {
        highScoreManager.SaveHightScore(playerScore);
    }

    void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = playerScore.ToString();
        }
    }

    void TriggerGameOverPanel()
    {
        gameOverPanel.SetActive(true);
    }

    public void IncreasePlayerScore(int coinScore)
    {
        playerScore += coinScore;
        scoreText.text = playerScore.ToString();
    }

    public void IncreasePlayerLives()
    {
        if (playerLives == 3)
        {
            StartCoroutine(LoadNoMoreLivesText());
        }
        else
        {
            lives[playerLives].gameObject.SetActive(true);
            playerLives++;
        }
    }

    IEnumerator LoadNoMoreLivesText()
    {
        livesText.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(delayTime);
        livesText.gameObject.SetActive(false);
    }

    public int getLiveCount() { return playerLives; }
   
    public void ResetGameSession()
    {
        ScenePersist.Instance.ResetScenePersist();
        SceneManager.LoadScene(1);
        Destroy(gameObject);
    }

    public void QuitGame()
    {
#if UNITY_WEBGL
        Application.ExternalEval("window.location.href='https://noychen1.itch.io/';");
#else
            Application.Quit();
#endif
    }

        void DesplayHighScore()
    {
        var score = highScoreManager.GetHighScore();
        highScore.text = "The high score is: " + score;
    }
    void DecreaseLife()
    {
        playerLives--;
        lives[playerLives].gameObject.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
