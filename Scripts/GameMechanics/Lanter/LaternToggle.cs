using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternToggle : MonoBehaviour
{
    [Header("On/Off")]
    public bool OnState;
    public bool Toggled;
    [Header("Light Variables")]
    public GameObject Light;
    public float EmissionPowerOn;
    public void Toggle()
    {
        if (!OnState)
        {
            OnState = true;
            Light.SetActive(true);
            GetComponent<Renderer>().material.SetFloat("_EmissionPower", EmissionPowerOn);
           
        }
        else if (OnState)
        {
            OnState = false;
            Light.SetActive(false);
            GetComponent<Renderer>().material.SetFloat("_EmissionPower", 0);
        }
    }
}
