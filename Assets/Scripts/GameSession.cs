using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GameSession : MonoBehaviour
{
    static GameSession instance;
    private IHighScoreManager highScoreManager;
    private IPlatformHandler platformHandler;


    [SerializeField] int playerLives = 3;
    [SerializeField] int playerScore = 0;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] GameObject[] lives;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] TextMeshProUGUI highScore;


    float delayTime = 1f;


    [Inject]
    public void Construct(IHighScoreManager highScoreManager, IPlatformHandler platformHandler)
    {
        this.highScoreManager = highScoreManager;
        this.platformHandler = platformHandler;
    }

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

    private void SetPlatformHandlerBasedOnPlatform()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.WebGLPlayer:
                platformHandler = new WebGLPlatformHandler();
                break;

            default:
                platformHandler = new DefaultPlatformHandler();
                break;
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            Destroy(this.gameObject); // Destroy duplicate instances
        }
    }
    void Start()
    {
        UpdateUI();
        UpdateLivesPanel();
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
        highScoreManager.SaveHighScore(playerScore);
    }

    void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = playerScore.ToString();
        }
    }

    void UpdateLivesPanel()
    {
        for (int i = 0; i < playerLives; i++)
        {
            lives[i].gameObject.SetActive(true);
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
            //**improve to tasks instead of coroutine
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
        platformHandler.QuitGame();   
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

        StartCoroutine(WaitForDie());
    }

    IEnumerator WaitForDie()
    {
        yield return new WaitForSeconds(delayTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
