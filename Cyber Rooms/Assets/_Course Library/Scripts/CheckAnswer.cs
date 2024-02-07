using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CheckAnswer : MonoBehaviour
{
    public bool isPhishing;
    public GameObject correctAnswerObject;
    public GameObject wrongAnswerObject;
    public GameObject noAnswerObject;
    public GameObject[] tooltips;
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
            if ((answer == "Phishing" && isPhishing) || (answer == "Legitimate" && !isPhishing))
            {
                noAnswerObject.SetActive(false);
                correctAnswerObject.SetActive(true);
            }
            else if ((answer == "Phishing" && !isPhishing) || (answer == "Legitimate" && isPhishing))
            {
                noAnswerObject.SetActive(false);
                wrongAnswerObject.SetActive(true);
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
        }
    }

    public void CheckOnMoveAway() {
        if (!correctAnswerObject.activeInHierarchy && !wrongAnswerObject.activeInHierarchy){
            noAnswerObject.SetActive(true);
        }
    }
}
