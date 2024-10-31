using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] int playerScore = 0;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] GameObject[] lives;

    float delayTime = 3f;
    private void Awake()
    {
        int numOfGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numOfGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Start()
    {
        scoreText.text = playerScore.ToString();
    }
    public void ProcessPlayerDeath()
    {
        if(playerLives > 1)
        {
            DecreaseLife();
        }
        else
        {
            ResetGameSession();
        }
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
    void ResetGameSession()
    {
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    void DecreaseLife()
    {
        playerLives--;
        lives[playerLives].gameObject.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
