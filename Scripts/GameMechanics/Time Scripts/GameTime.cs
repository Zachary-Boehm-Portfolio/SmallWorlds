using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTime : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Game Time")]
    public ClockScript ClockTime;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log(ClockTime.clock.GetTime().ToString());
        }
    }
}
