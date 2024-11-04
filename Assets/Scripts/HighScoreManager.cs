using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreManager
{
    
    public void SaveHightScore(int score)
    {
        int currentHighScore = PlayerPrefs.GetInt("RecordScore", 0);
        if (score > currentHighScore)
        {
            PlayerPrefs.SetInt("RecordScore", score);
            PlayerPrefs.Save();
        }
    }

    public int GetHighScore()
    {
        int highScore = PlayerPrefs.GetInt("RecordScore", 0);
        return highScore;
    }
}
