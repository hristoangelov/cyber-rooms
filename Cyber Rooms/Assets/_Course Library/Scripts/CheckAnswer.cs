using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CheckAnswer : MonoBehaviour
{
    public bool isPhishing;
    public GameObject emailObject;
    public GameObject correctAnswerObject;
    public GameObject wrongAnswerObject;
    public GameObject noAnswerObject;
    public GameObject[] tooltips;
    public GameObject ScoreTracker;
    public AudioSource source;
    public AudioClip wrongAnswerSound;
    public AudioClip correctAnswerSound;
    protected bool isInside { get; set; }
    protected string answer { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        isInside = true;
        if (other.gameObject.tag == "Legitimate")
        {
            answer = "Legitimate";
        }
        else if (other.gameObject.tag == "Phishing")
        {
            answer = "Phishing";
        }
        else
        {
            answer = "Other";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isInside = false;
    }

    public void CheckOnDrop()
    {
        if (isInside)
        {
            //correct answer
            if ((answer == "Phishing" && isPhishing) || (answer == "Legitimate" && !isPhishing))
            {
                noAnswerObject.SetActive(false);
                correctAnswerObject.SetActive(true);
                ScoreTracker.GetComponent<ScoreTracker>().correct += 1;
                source.PlayOneShot(correctAnswerSound);
            }
            //wrong answer
            else if ((answer == "Phishing" && !isPhishing) || (answer == "Legitimate" && isPhishing))
            {
                noAnswerObject.SetActive(false);
                wrongAnswerObject.SetActive(true);
                source.PlayOneShot(wrongAnswerSound);
                foreach (GameObject tooltip in tooltips)
                {
                    tooltip.SetActive(true);
                }
            }
            else
            {
                return;
            }
            GetComponent<XRBaseInteractable>().interactionLayers = InteractionLayerMask.GetMask("Nothing");
            ScoreTracker.GetComponent<ScoreTracker>().answered += 1;
        }
    }

    public void CheckOnMoveAway()
    {
        if (!correctAnswerObject.activeInHierarchy && !wrongAnswerObject.activeInHierarchy && emailObject.activeInHierarchy)
        {
            noAnswerObject.SetActive(true);
        }
    }
}
