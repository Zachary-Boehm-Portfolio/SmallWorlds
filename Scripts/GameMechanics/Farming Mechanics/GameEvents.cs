using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;
    private void Awake()
    {
        current = this;
    }

    public event Action<System.TimeSpan> onTimeChangeTriggerEnter;
    public void OnTimeChangeTriggerEnter(System.TimeSpan _time)
    {
        if(onTimeChangeTriggerEnter != null)
        {
            onTimeChangeTriggerEnter(_time);
        }
    }
}
