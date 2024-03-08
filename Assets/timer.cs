using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class timer : NetworkBehaviour
{
    public bool started;
    public bool running;
    public bool complete;
    public TMPro.TMP_Text timerText;

    private float startTime;

    // Start is called before the first frame update
    void Start()
    {
        started = true;
        running = false;
        complete = false;

        timerText = NetworkManager.singleton.transform.Find("UI").Find("timer").GetComponent<TMPro.TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (started && !running)
        {
            startTime = Time.time;
            started = false;
            running = true;
        } 

        if (running && !complete)
        {
            timerText.text = (Time.time - startTime).ToString("F2");
        }
    }
}
