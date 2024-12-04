using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelExit : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 0.5f;
    private bool isLevelLoading = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isLevelLoading)
        {
            isLevelLoading = true;
            LoadNextLevel();
        }
    }

    private async void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        await WaitForSecondsRealtime(levelLoadDelay);

        if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            GameSession.Instance.EndGame();
        }
        else
        {
            ScenePersist.Instance.ResetScenePersist();
            SceneManager.LoadScene(nextSceneIndex);
        }

        isLevelLoading = false;
    }

    private async Task WaitForSecondsRealtime(float seconds)
    {
        float targetTime = Time.realtimeSinceStartup + seconds;
        while (Time.realtimeSinceStartup < targetTime)
        {
            await Task.Yield();
        }
    }
}


