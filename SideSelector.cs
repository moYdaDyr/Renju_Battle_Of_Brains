using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideSelector : MonoBehaviour
{
    public void SetSide(int side)
    {
        if (side == -1)
        {
            GameData.playerMove = GameData.rnd.Next(0,1);
        }
        else
        {
            GameData.playerMove = side;
        }
    }

    void Start()
    {
        GameData.playerMove = 1;
    }
}
