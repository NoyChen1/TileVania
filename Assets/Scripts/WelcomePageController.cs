using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WelcomePageController : MonoBehaviour
{
    public void OnStartGameButtonPressed()
    {
        SceneManager.LoadScene("Level1");
    }

}