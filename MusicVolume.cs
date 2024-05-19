using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MusicVolume : MonoBehaviour
{
    public Slider commonSlider;
    public Slider musicSlider;
    public Slider gameSoundSlider;

    public AudioMixer au;

    

    public void commonVolumeChanged()
    {
        au.SetFloat("MasterVolume", commonSlider.value);
        Debug.Log("MasterVolume "+ commonSlider.value);
    }

    public void musicVolumeChanged()
    {
        au.SetFloat("MusicVolume", musicSlider.value);
        Debug.Log("MusicVolume " + musicSlider.value);
    }

    public void gameVolumeChanged()
    {
        au.SetFloat("SoundEffectVolume", gameSoundSlider.value);
        Debug.Log("SoundEffectVolume " + gameSoundSlider.value);
    }
}
