using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class PhysicsButton : MonoBehaviour
{
    [SerializeField] private GameObject button;
    [SerializeField] private GameObject validationMessageBackground;
    [SerializeField] private TMP_Text validationMessage;
    public UnityEvent onRelease;
    AudioSource sound;

    [Header("Leaderboard")]
    [SerializeField] private GameObject leaderboardPrompt;
    [SerializeField] private GameObject timer;
    private bool isPressed;

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
        //stop timer of the game
        timer.GetComponent<Timer>().StopTimer();
        onRelease.Invoke();
    }
}
