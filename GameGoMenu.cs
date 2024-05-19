using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameGoMenu : MonoBehaviour
{
    public void GoMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void GoGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void GoGameSettings()
    {
        SceneManager.LoadScene("GameSettings");
    }

    public void GoLoad()
    {
        SceneManager.LoadScene("LoadGame");
    }

    public void GoGameInit()
    {
        GameData.changePlayerMove = false;
        GameData.Init(-1, -1);
        SceneManager.LoadScene("Game");
    }
}
