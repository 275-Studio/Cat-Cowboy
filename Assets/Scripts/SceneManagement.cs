using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{

    public void GoToGameScene()
    {
        SceneManager.LoadScene("Scenes/Game Scene");
    }

    public void GoToMenuScene()
    {
        SceneManager.LoadScene("Scenes/Main Menu");
    }
}
