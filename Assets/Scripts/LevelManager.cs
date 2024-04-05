using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void LoadScene(int n)
    {
        SceneManager.LoadScene(n);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
