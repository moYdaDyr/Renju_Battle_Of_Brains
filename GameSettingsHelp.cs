using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettingsHelp : MonoBehaviour
{
    public static bool showHelp = false;

    public GameObject aiSettingWindow;
    public GameObject helpAISetting;
    public GameObject helpOrig;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Debug.Log("Help called");
            showHelp = !showHelp;
        }
        if (aiSettingWindow.activeSelf)
        {
            helpAISetting.SetActive(showHelp);
        }
        else
        {
            helpOrig.SetActive(showHelp);
        }
    }

    public static void ToggleShowHelp()
    {
        showHelp = !showHelp;
    }

    void Start()
    {
        GameData.gameType = 0;
    }
}
