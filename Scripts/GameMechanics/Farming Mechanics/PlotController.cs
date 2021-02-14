using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotController : MonoBehaviour
{
    public FarmController Controller;
    private GameObject Player;
    public void Start()
    {
        Player = GameObject.Find("Player");
    }
    public void CropType(int _type)
    {
        Controller.Plant(_type);
        Player.GetComponent<Controls>().ToggleCursor(false);
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
