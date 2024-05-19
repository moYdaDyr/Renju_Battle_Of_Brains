using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHelp : MonoBehaviour
{
    public static bool showHelp = false;

    public GameObject victoryWindow;
    public GameObject drawWindow;
    public GameObject saveMenuWindow;
    public GameObject rewriteErrorWindow;
    public GameObject exitWithoutSaveWindow;

    public GameObject victoryHelp;
    public GameObject drawHelp;
    public GameObject saveMenuHelp;
    public GameObject helpOrig;
    public GameObject rewriteErrorHelp;
    public GameObject exitWithoutSaveHelp;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Debug.Log("Help called");
            showHelp = !showHelp;
        }
        if (victoryWindow.activeSelf)
        {
            victoryHelp.SetActive(showHelp);
        }
        else if (drawWindow.activeSelf)
        {
            drawHelp.SetActive(showHelp);
        }
        else if (saveMenuWindow.activeSelf && !rewriteErrorWindow.activeSelf)
        {
            saveMenuHelp.SetActive(showHelp);
        }
        else if (saveMenuWindow.activeSelf && rewriteErrorWindow.activeSelf)
        {
            rewriteErrorHelp.SetActive(showHelp);
        }
        else if (exitWithoutSaveWindow.activeSelf)
        {
            exitWithoutSaveHelp.SetActive(showHelp);
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
}
