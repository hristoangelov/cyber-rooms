using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Wait : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(waitForIntro());
    }

    IEnumerator waitForIntro()
    {
        yield return new WaitForSeconds(11);

        SceneManager.LoadScene(1);
    }
}
