using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoosingGameType : MonoBehaviour
{
    public void SetType(int type)
    {
        GameData.Init(type,-1);
    }
}
