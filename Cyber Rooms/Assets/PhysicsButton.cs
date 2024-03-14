using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class PhysicsButton : MonoBehaviour
{
    [SerializeField] private GameObject button;
    [SerializeField] private GameObject validationMessageBackground;
    [SerializeField] private TMP_Text validationMessage;
    [SerializeField] private ParticleSystem confetti;
    public UnityEvent onRelease;
    AudioSource sound;
    public AudioSource confettiSound;
    public AudioClip confettiClip;
    public CloudSaveScript onSaveScript;

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

        // start confetti particles and sound
        confetti.Play();
        confettiSound.clip = confettiClip;
        confettiSound.Play();

        leaderboardPrompt.SetActive(true);
        // stop timer of the game
        timer.GetComponent<Timer>().StopTimer();

        // save total time to cloud save
        float timeTaken = timer.GetComponent<Timer>().timer;
        onSaveScript.SaveData("time_in_seconds", timeTaken.ToString());
        onRelease.Invoke();
    }
}
