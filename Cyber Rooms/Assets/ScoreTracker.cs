using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreTracker : MonoBehaviour
{
    public TMP_Text Answered;
    public TMP_Text Correct;
    public int answered = 0;
    public int correct = 0;
    void Update()
    {
        //Updates text mesh component
        Answered.SetText("Answered: " + answered.ToString() + "/4");
        Correct.SetText("Correct: " + correct.ToString() + "/4");
    }
}
