using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class timer : MonoBehaviour
{
    private float stopwatchTime;
    [SerializeField] private TMP_Text display;
    public static TimeSpan time;

    // Start is called before the first frame update
    void Start()
    {
        stopwatchTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        stopwatchTime += Time.deltaTime;
        time = TimeSpan.FromSeconds(stopwatchTime);
        display.text = time.Minutes.ToString() + ":" + time.Seconds.ToString() + ":" + time.Milliseconds.ToString();

    }

}
