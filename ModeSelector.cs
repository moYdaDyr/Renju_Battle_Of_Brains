using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ModeSelector : MonoBehaviour
{
    public GameObject Settings;

    public void WhatPageNext()
    {
        if (GameData.gameMode == 0)
        {
            Settings.SetActive(true);
        }
        else
        {
            SceneManager.LoadScene("Game");
        }
    }

    public void SetMode(int mode)
    {
        Debug.Log("game mode selected " + mode);
        GameData.gameMode = mode;
    }
}
