using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CheckAnswer : MonoBehaviour
{
    public bool isPhishing;
    public GameObject correctAnswerObject;
    public GameObject wrongAnswerObject;
    protected bool isInside { get; set; }
    protected string answer { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        isInside = true;
        if (other.gameObject.tag == "Legitimate"){
            answer = "Legitimate";
        }else if (other.gameObject.tag == "Phishing"){
            answer = "Phishing";
        }
        else{
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
            if ((answer == "Phishing" && isPhishing) || (answer == "Legitimate" && !isPhishing))
            {
                correctAnswerObject.SetActive(true);
            }
            else if ((answer == "Phishing" && !isPhishing) || (answer == "Legitimate" && isPhishing))
            {
                wrongAnswerObject.SetActive(true);
            }
            else{
                return;
            }
        }
    }
}
