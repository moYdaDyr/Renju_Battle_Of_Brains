using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Knot.Localization;

public class Settings : MonoBehaviour
{
    static bool isFullScreen;

    public Toggle isFullScreenT;

    public void FullScreenToggle()
    {
        isFullScreen = !isFullScreen;
        isFullScreenT.isOn = isFullScreen;
        Screen.fullScreen = isFullScreen;
    }

    public AudioMixer am;
    public void AudioVolume(float sliderValue)
    {
        am.SetFloat("audioBackVolume", sliderValue);
    }

    static public (float x, float y) currentResolution;
    
    static Resolution[] rsl;
    static List<string> resolutions;
    public Dropdown resDrop;
    List<string> localizations;
    public Dropdown locDrop;
    
    public void Start()
    {
        isFullScreen = true;
        isFullScreenT.isOn = true;
        // разрешение
        resolutions = new List<string>();
        rsl = Screen.resolutions;
        Array.Reverse(rsl);
        foreach (var i in rsl)
        {
            resolutions.Add(i.width + "x" + i.height);
        }
        resDrop.ClearOptions();
        resDrop.AddOptions(resolutions);

        //Resolution();

        // локализация

        locDrop.ClearOptions();
        localizations = KnotLocalization.Manager.Languages.Select(l => l.NativeName).ToList();
        locDrop.AddOptions(localizations);
        locDrop.SetValueWithoutNotify(KnotLocalization.Manager.Languages.IndexOf(KnotLocalization.Manager.SelectedLanguage));
        locDrop.onValueChanged.AddListener(arg0 =>
        {
            KnotLocalization.Manager.LoadLanguage(KnotLocalization.Manager.Languages[arg0]);
        });

    }

    public void Resolution()
    {
        int r = resDrop.value;

        Screen.SetResolution(rsl[r].width, rsl[r].height, isFullScreen);
        
        currentResolution.x = rsl[r].width;
        currentResolution.y = rsl[r].height;
        //Debug.Log(rsl[r].width + "x" + rsl[r].height);
    }
}
