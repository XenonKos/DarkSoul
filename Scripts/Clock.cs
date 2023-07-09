using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField]
    private Transform hoursPivot, minutesPivot, secondsPivot;

    const float hoursToDegrees = -30f, minutesToDegrees = -6f, secondsToDegrees = -6f;

    private void Awake()
    {
        hoursPivot.localRotation = Quaternion.Euler(0f, 0f, hoursToDegrees * (DateTime.Now.Hour + DateTime.Now.Minute / 60f));
        minutesPivot.localRotation = Quaternion.Euler(0f, 0f, minutesToDegrees * (DateTime.Now.Minute + DateTime.Now.Second / 60f));
        secondsPivot.localRotation = Quaternion.Euler(0f, 0f, secondsToDegrees * DateTime.Now.Second);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TimeSpan time = DateTime.Now.TimeOfDay;
        hoursPivot.localRotation = Quaternion.Euler(0f, 0f, hoursToDegrees * (float)time.TotalHours);
        minutesPivot.localRotation = Quaternion.Euler(0f, 0f, minutesToDegrees * (float)time.TotalMinutes);
        secondsPivot.localRotation = Quaternion.Euler(0f, 0f, secondsToDegrees * (float)time.TotalSeconds);
    }
}
