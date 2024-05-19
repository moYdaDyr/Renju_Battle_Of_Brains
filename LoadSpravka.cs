using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadSpravka : MonoBehaviour
{
    public void clickHelp()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "spravka.chm");
        System.Diagnostics.Process.Start(filePath);
    }
}
