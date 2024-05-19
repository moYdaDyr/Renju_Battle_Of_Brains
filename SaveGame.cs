using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveGame : MonoBehaviour
{
    string fileName;

    string currentDirectory;

    bool needsRewrite;

    public InputField nameInput;

    public GameObject error;

    public GameObject saveMenu;

    void Start()
    {
        currentDirectory = Directory.GetCurrentDirectory();

        if (!Directory.Exists(currentDirectory + "\\SavedGames"))
            Directory.CreateDirectory(currentDirectory + "\\SavedGames");

        var input = nameInput.GetComponent<InputField>();
        var se = new InputField.OnChangeEvent();
        se.AddListener(SetFileName);
        input.onValueChange = se;
    }

    public void SetFileName(string arg0)
    {
        Debug.Log(arg0);
        fileName = arg0;
    }

    public void WillAskForSave()
    {
        if (!GameData.IsVictory(GameData.currentMove+1)) saveMenu.SetActive(true);
    }

    public void AskForSaving()
    {
        
        Debug.Log(currentDirectory + "\\SavedGames\\" + fileName + ".rnj");
        if (File.Exists(currentDirectory + "\\SavedGames\\" + fileName+".rnj"))
        {
            error.SetActive(true);
            return;
        }
        GameData.WriteToFile(currentDirectory + "\\SavedGames\\" + fileName);
        saveMenu.SetActive(false);
    }

    public void SaveThisGame()
    {
        Debug.Log(currentDirectory + "\\SavedGames\\" + fileName + ".rnj");
        GameData.WriteToFile(currentDirectory + "\\SavedGames\\" + fileName);
    }
}
