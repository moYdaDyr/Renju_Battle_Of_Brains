using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHelp : MonoBehaviour
{
    public static bool showHelp = false;

    public GameObject settingsWindow;
    public GameObject helpSettings;
    public GameObject helpOrig;

    public GameObject settings;
    public GameObject trueSettings;

    public Texture2D defaultCursorTexture;

    void Start()
    {
        Cursor.SetCursor(defaultCursorTexture, new Vector3(0, 0, 0), CursorMode.Auto);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Debug.Log("Help called");
            showHelp = !showHelp;
        }
        if (settingsWindow.activeSelf)
        {
            helpSettings.SetActive(showHelp);
        }
        else
        {
            helpOrig.SetActive(showHelp);
        }
    }

    public void ToggleShowHelp()
    {
        showHelp = !showHelp;
    }
}
