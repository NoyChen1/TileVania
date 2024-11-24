using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreManager : IHighScoreManager
{
    
    public void SaveHighScore(int highScore)
    {
        int currentHighScore = PlayerPrefs.GetInt("RecordScore", 0);
        if (highScore > currentHighScore)
        {
            PlayerPrefs.SetInt("RecordScore", highScore);
            PlayerPrefs.Save();
        }
    }

    public int GetHighScore()
    {
        int highScore = PlayerPrefs.GetInt("RecordScore", 0);
        return highScore;
    }
}
