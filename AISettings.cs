using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AISettings : MonoBehaviour
{
    public Slider aggresivnessSlider;
    public Slider difficultySlider;

    // Start is called before the first frame update
    void Start()
    {
        aggresivnessSlider.value = 1;
        difficultySlider.value = 1;
    }

    public void SetAggresivness()
    {
        GameData.aggressivness = 1.3 - aggresivnessSlider.value*0.4;
    }

    public void SetDifficulty()
    {
        GameData.difficulty = (int)difficultySlider.value;
    }
}
