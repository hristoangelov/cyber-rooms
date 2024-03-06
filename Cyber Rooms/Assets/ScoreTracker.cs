using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ScoreTracker : MonoBehaviour
{
    public TMP_Text Answered;
    public TMP_Text Correct;
    public int answered = 0;
    public int correct = 0;
    public GameObject completedChapterObject;
    public TMP_Text completedChapterText;
    private readonly float distance = 1.25f;
    void Update()
    {
        //Updates text mesh component
        Answered.SetText("Answered: " + answered.ToString() + "/4");
        Correct.SetText("Correct: " + correct.ToString() + "/4");
        if (answered == 4 && !completedChapterObject.IsDestroyed())
        {
            ShowCompletedText();
        }
    }

    void ShowCompletedText()
    {
        // Displays UI component in front of camera when all questions are answered
        Vector3 position = Camera.main.transform.TransformPoint(Vector3.forward * distance);
        completedChapterObject.transform.position = position;
        completedChapterObject.transform.rotation = Camera.main.transform.rotation;
        if (correct == 1)
        {
            completedChapterText.SetText("You answered " + correct.ToString() + " out of 4 question correct. Press Next to go to the next challenge");
        }
        else
        {
            completedChapterText.SetText("You answered " + correct.ToString() + " out of 4 questions correct. Press Next to go to the next challenge");
        }
        completedChapterObject.SetActive(true);
    }

    public void HideCompletedText()
    {
        Destroy(completedChapterObject);
    }
}
