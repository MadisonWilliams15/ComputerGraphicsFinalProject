using UnityEngine;
using System;
using UnityEngine.UI;

public class ClockScript : MonoBehaviour
{
    private Text textClock;
    private DateTime startTime;
    void Awake()
    {
        textClock = GetComponent<Text>();
        startTime = DateTime.Now;
    }
    void Update()
    {
        TimeSpan time = DateTime.Now.Subtract(startTime);
        string hour = LeadingZero(time.Hours);
        string minute = LeadingZero(time.Minutes);
        string second = LeadingZero(time.Seconds);
        textClock.text = hour + ":" + minute + ":" + second;
    }
    string LeadingZero(int n)
    {
        return n.ToString().PadLeft(2, '0');
    }
}
