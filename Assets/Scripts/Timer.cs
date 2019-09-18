using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private float startTime;
    public Text timerText;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
            DontDestroyOnLoad(transform.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        float t = Time.time - startTime;

        string minutes = ((int) t / 60).ToString();
        string seconds = (t % 60).ToString("f2");

        timerText.text = minutes + ":" + seconds;
    }

    public void GetTime()
    {
        float t = Time.time - startTime;

        string minutes = ((int)t / 60).ToString("f2");
        string seconds = (t % 60).ToString("f2");
    }
}
