using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SaveLoadScript : MonoBehaviour
{
    public Dropdown FileSelector;

    public GameObject CorruptWarning;

    public GameObject ParametersPlaceholder;
    public GameObject NoSaves;

    public GameObject Parameters;
    public GameObject AIParameters;

    public Text PDate;

    public GameObject Ptype;

    public GameObject Ptype0;
    public GameObject Ptype1;
    public GameObject Ptype2;

    public GameObject Pmode;

    public GameObject Pmode0;
    public GameObject Pmode1;

    // Start is called before the first frame update
    void Start()
    {
        GameData.currentDirectory = Directory.GetCurrentDirectory();

        if (!Directory.Exists(GameData.currentDirectory + "\\SavedGames"))
            Directory.CreateDirectory(GameData.currentDirectory +"\\SavedGames");

        savedGames = new List<string>();

        Debug.Log(GameData.currentDirectory + "\\SavedGames");

        NoSaves.SetActive(true);

        GetSaveNames();

        LoadFile(0);
    }

    List<string> savedGames;
    string[] ss;

    public void GetSaveNames()
    {
        savedGames.Clear();

        FileSelector.ClearOptions();

        ss = Directory.GetFiles(GameData.currentDirectory +"\\SavedGames", "*.rnj");

        if (ss.GetLength(0) != 0) NoSaves.SetActive(false);

        for (int i = 0; i < ss.Length; i++)
        {
            savedGames.Add(Path.GetFileNameWithoutExtension(ss[i]));
            Debug.Log(savedGames[i]);
        }

        savedGames.Sort();

        FileSelector.AddOptions(savedGames);
    }

    public void LoadFile(int mode = 0)
    {
        Debug.Log("load begin " + GameData.gameMode + " " + mode);
        if (savedGames.Count > 0)
        {
            int f = FileSelector.value;

            Debug.Log("current game mode load begin " + GameData.gameMode + " " + mode);

            if (!GameData.ReadFromFile(ss[f]))
            {
                if (mode != 0)
                    CorruptWarning.SetActive(true);
                ParametersPlaceholder.SetActive(true);
            }
            else
            {
                ParametersPlaceholder.SetActive(false);
                Parameters.SetActive(true);

                //Destroy(Ptype);
                //Destroy(Pmode);

                if (GameData.gameType == 0)
                {
                    Ptype1.SetActive(false);
                    Ptype2.SetActive(false);
                    Ptype0.SetActive(true);
                }
                else if (GameData.gameType == 1)
                {
                    Ptype0.SetActive(false);
                    Ptype2.SetActive(false);
                    Ptype1.SetActive(true);
                }
                else if (GameData.gameType == 2)
                {
                    Ptype0.SetActive(false);
                    Ptype1.SetActive(false);
                    Ptype2.SetActive(true);
                }

                if (GameData.decorativeGameMode == 0)
                {
                    Pmode1.SetActive(false);
                    Pmode0.SetActive(true);
                }
                else
                {
                    Pmode0.SetActive(false);
                    Pmode1.SetActive(true);
                }

                PDate.text = GameData.date.ToString();

                Debug.Log("current game mode load end " + GameData.gameMode + " " + mode);

                if (mode > 1)
                {
                    if (GameData.aggressivness == 0 && GameData.gameMode == 0)
                    {
                        AIParameters.SetActive(true);
                        Debug.Log("AI parameters");
                    }
                    else
                    {
                        SceneManager.LoadScene("Game");
                    }
                        
                }
                    
            }
        }
    }
}
