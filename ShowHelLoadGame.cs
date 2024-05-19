using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHelLoadGame : MonoBehaviour
{
    public static bool showHelp = false;

    public GameObject errorWindow;
    public GameObject aiWindow;
    public GameObject aiHelp;
    public GameObject helpError;
    public GameObject helpOrig;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Debug.Log("Help called");
            showHelp = !showHelp;
        }
        if (errorWindow.activeSelf)
        {
            helpError.SetActive(showHelp);
        }
        else if (aiWindow.activeSelf)
        {
            aiHelp.SetActive(showHelp);
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
