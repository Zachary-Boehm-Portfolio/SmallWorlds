using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaternToggle : MonoBehaviour
{
    [Header("On/Off")]
    public bool OnState;
    [Header("Light Variables")]
    public GameObject Light;
    public float EmissionPowerOn;
    private void Start()
    {
        
    }
    void Update()
    {
        if (OnState)
        {
            Debug.Log("Light On");
            Light.SetActive(true);
            GetComponent<Renderer>().material.SetFloat("_EmissionPower", EmissionPowerOn);
        }
        else if(!OnState)
        {
            Light.SetActive(false);
            GetComponent<Renderer>().material.SetFloat("_EmissionPower", 0f);
        }
    }
}
