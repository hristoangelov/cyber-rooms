using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Keyboard;
using Newtonsoft.Json;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;
using UnityEditor.Rendering;

public class UpdateLeaderboard : MonoBehaviour
{
    [SerializeField] private GameObject timer;
    [SerializeField] private GameObject phishingCorrectAnswers;
    [SerializeField] private GameObject usernamePrompt;
    [SerializeField] private TextMeshProUGUI[] topScores;
    private int timeWeight = 300;
    private int minimumScore = 20;

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
        // need to uncheck the "Force Remove Internet" from Project Setting -> XR Plug-in Management -> Open XR to work on headset
        StartCoroutine(GetLeaderboard());
    }

    IEnumerator GetLeaderboard()
    {
        yield return SetScore();
        yield return GetScore();
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

        string url = "http://dreamlo.com/lb/65e0e8f28f40bbbe889ee7be/json/5";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            string responseData = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);
            DreamloClasses response = JsonConvert.DeserializeObject<DreamloClasses>(responseData);
            UpdateCurrentLeaderboard(response);

            Debug.Log("Get scores");
        }
    }

    public void UpdateCurrentLeaderboard(DreamloClasses response)
    {
        foreach (var score in topScores)
        {
            score.text = "";
        }

        if (response.dreamlo.leaderboard.entry[0].name != null)
        {
            topScores[0].text = $"1. {response.dreamlo.leaderboard.entry[0].name} - {response.dreamlo.leaderboard.entry[0].score}";
        }
        if (response.dreamlo.leaderboard.entry[1].name != null)
        {
            topScores[1].text = $"2. {response.dreamlo.leaderboard.entry[1].name} - {response.dreamlo.leaderboard.entry[1].score}";
        }
        if (response.dreamlo.leaderboard.entry[2].name != null)
        {
            topScores[2].text = $"3. {response.dreamlo.leaderboard.entry[2].name} - {response.dreamlo.leaderboard.entry[2].score}";
        }
        if (response.dreamlo.leaderboard.entry[3].name != null)
        {
            topScores[3].text = $"4. {response.dreamlo.leaderboard.entry[3].name} - {response.dreamlo.leaderboard.entry[3].score}";
        }
        if (response.dreamlo.leaderboard.entry[4].name != null)
        {
            topScores[4].text = $"5. {response.dreamlo.leaderboard.entry[4].name} - {response.dreamlo.leaderboard.entry[4].score}";
        }
    }

}
