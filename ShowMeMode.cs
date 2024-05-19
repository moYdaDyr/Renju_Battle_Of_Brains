using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowMeMode : MonoBehaviour
{
    public void ShowMeModeF()
    {
        Debug.Log("current game mode value changed" + GameData.gameMode);
    }
}
