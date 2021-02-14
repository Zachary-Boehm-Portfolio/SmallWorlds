using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    private void Awake()
    {
        Screen.SetResolution(1920, 1080, true);
    }
    [Header("Sun and Moon")]
    public GameObject SunAndMoon;
    public GameObject TimeUI;
    public ClockScript GameTime;
    public static System.TimeSpan CurrTime;
    public float degrees;
    void FixedUpdate()
    {
        CurrTime = GameTime.clock.GetTime();
        degrees = (float)(((CurrTime.Hours * 60f * 60f) + (CurrTime.Minutes * 60f) + CurrTime.Seconds) / 240f);
        SunAndMoon.transform.localRotation = Quaternion.Euler(degrees, 0, 0);
        TimeUI.transform.localRotation = Quaternion.Euler(0, 0, -degrees);
    }
    private void Update()
    {
        if (((CurrTime.Hours * 60f * 60f) + (CurrTime.Minutes * 60f) + CurrTime.Seconds) % 800 == 0)
        {
            try
            {
                GameEvents.current.OnTimeChangeTriggerEnter(CurrTime);
            }catch(Exception e)
            {
                Debug.Log(e.Message);
            }
        }
    }
}
