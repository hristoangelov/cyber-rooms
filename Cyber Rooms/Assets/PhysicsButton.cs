using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class PhysicsButton : MonoBehaviour
{
    [SerializeField] private GameObject button;
    [SerializeField] private GameObject validationMessageBackground;
    [SerializeField] private TMP_Text validationMessage;
    [SerializeField] private GameObject phishingCorrectAnswers;
    [SerializeField] private GameObject timer;
    [SerializeField] private GameObject leaderboardPrompt;
    public UnityEvent onRelease;
    private bool isPressed;
    private int timeWeight = 300;
    private int minimumScore = 20;
    AudioSource sound;

    void Start()
    {
        isPressed = false;
        sound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter()
    {
        if (!isPressed)
        {
            button.transform.localPosition = new Vector3(0.003f, 0.003f, 0);
            isPressed = true;
            sound.Play();
        }
    }

    private void OnTriggerExit()
    {
        // return button position
        button.transform.localPosition = new Vector3(0.02f, 0.02f, 0);
        isPressed = false;

        // show congratulations tooltip
        validationMessageBackground.SetActive(true);
        validationMessage.SetText("Great job! The attackers just decided that if even possible, attacking your company will be too hard!\nYou are now prepared to defend both your personal and company's data.");
        validationMessage.color = new Color(0, 255, 0, 255);

        leaderboardPrompt.SetActive(true);
        //stop timer and get the final score of the player
        timer.GetComponent<Timer>().StopTimer();
        float finalSCore = CalculateFinalScore();
        onRelease.Invoke();
    }

    private float CalculateFinalScore()
    {
        //get phsihing score and the timer value
        float timeTaken = timer.GetComponent<Timer>().timer;
        int correctAnswers = phishingCorrectAnswers.GetComponent<ScoreTracker>().correct;
        float finalScore =  (correctAnswers + (timeWeight / timeTaken))*10;
        return minimumScore + finalScore;
    }
}
