using UnityEditor;
using UnityEngine;

public interface IPlatformHandler
{
    void QuitGame();
}


public class WebGLPlatformHandler : IPlatformHandler
{
    public void QuitGame()
    {
        Application.ExternalEval("window.location.href='https://noychen.itch.io/';");
    }
}

public class DefaultPlatformHandler : IPlatformHandler
{
    public void QuitGame()
    {
        //Application.Quit();
        Debug.Log("Quitting not supported on this platform.");
    }
}