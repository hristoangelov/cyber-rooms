using UnityEngine;

public class Timer : MonoBehaviour
{
    public float timer = 0.0f;
    public bool isTimer = false;

    void Update()
    {
        if (isTimer)
        {
            timer += Time.deltaTime;
        }
    }

    public void StartTimer()
    {
        isTimer = true;
    }

    public void StopTimer()
    {
        isTimer = false;
    }
}
