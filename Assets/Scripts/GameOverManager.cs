using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public void Reset()
    {
        SceneManager.LoadScene(0);
    }

    public void Menu()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
    }
}
