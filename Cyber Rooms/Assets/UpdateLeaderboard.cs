using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Keyboard;
using Newtonsoft.Json;
using TMPro;
using System;

public class UpdateLeaderboard : MonoBehaviour
{
    [SerializeField] private GameObject timer;
    [SerializeField] private GameObject phishingCorrectAnswers;
    [SerializeField] private GameObject usernamePrompt;
    [SerializeField] private TextMeshProUGUI[] topScores;
    [SerializeField] private TMP_Text leaderboardHeader;
    
    [Header("On Leaderboard Update")]
    [SerializeField] private GameObject validationMessageBackground;
    [SerializeField] private TMP_Text validationMessage;
    [SerializeField] private GameObject keyboardObject;
    [SerializeField] private GameObject leaderboardObject;
    [SerializeField] private GameObject usernamePromptObject;
    private int timeWeight = 300;
    private int minimumScore = 20;
    public int position;

    private float CalculateFinalScore()
    {
        //get phsihing score and the timer value
        float timeTaken = timer.GetComponent<Timer>().timer;
        int correctAnswers = phishingCorrectAnswers.GetComponent<ScoreTracker>().correct;
        float finalScore = minimumScore + (correctAnswers + (timeWeight / timeTaken)) * 10;
        return finalScore;
    }

    public void GetCurrentLeaderboard()
    {
        if (usernamePrompt.GetComponent<KeyboardManager>().outputField.text == "")
        {
            validationMessageBackground.SetActive(true);
            validationMessage.SetText("You need to enter your username first.");
            validationMessage.color = new Color(255, 0, 0, 255);
        }
        else{
            keyboardObject.SetActive(false);
            leaderboardObject.SetActive(true);
            usernamePromptObject.SetActive(false);
            // need to uncheck the "Force Remove Internet" from Project Setting -> XR Plug-in Management -> Open XR to work on headset
            StartCoroutine(GetLeaderboard());
        }
    }

    IEnumerator GetLeaderboard()
    {
        yield return SetScore();
        yield return GetScore();

        // update header of leaderboard
        string score = CalculateFinalScore().ToString();
        leaderboardHeader.text = $"You scored <i>{score}</i> points and are on <i>position {position}</i>";
    }

    public IEnumerator SetScore()
    {
        string username = usernamePrompt.GetComponent<KeyboardManager>().outputField.text;
        string score = CalculateFinalScore().ToString();
        string url = $"http://dreamlo.com/lb/2DMLtaZM7kitsygRgyXyUQlatAFe6dkkyNgP3blrC9_Q/add/{username}/{score}";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();
        }
    }

    public IEnumerator GetScore()
    {
        string username = usernamePrompt.GetComponent<KeyboardManager>().outputField.text;

        string url = "http://dreamlo.com/lb/65e0e8f28f40bbbe889ee7be/json";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            string responseData = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);
            DreamloClasses response = JsonConvert.DeserializeObject<DreamloClasses>(responseData);
            UpdateCurrentLeaderboard(response);

            for (int i = 0; i < response.dreamlo.leaderboard.entry.Count; i++)
            {
                if (response.dreamlo.leaderboard.entry[i].name == username)
                {
                    position = i + 1;
                    break;
                }
            }
        }
    }

    public void UpdateCurrentLeaderboard(DreamloClasses response)
    {
        foreach (var score in topScores)
        {
            score.text = "";
        }

        for (int i = 0; i < Math.Min(response.dreamlo.leaderboard.entry.Count, 5); i++)
        {
            topScores[i].text = $"{i + 1}. {response.dreamlo.leaderboard.entry[i].name} - {response.dreamlo.leaderboard.entry[i].score}";
        }
    }

}
