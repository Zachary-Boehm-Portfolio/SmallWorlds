using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackbackTabSelect : MonoBehaviour
{
    private int currentTab = 0;

    // Update is called once per frame
    void Update()
    {
        if (transform.GetChild(0).gameObject.activeSelf)
        {
            if (Input.GetButtonDown("ToolRight") && currentTab != 2)
            {
                currentTab++;
                GetComponent<BackpackController>().ToggleMenu(currentTab);
            }
            else if (Input.GetButtonDown("ToolLeft") && currentTab != 0)
            {
                currentTab--;
                GetComponent<BackpackController>().ToggleMenu(currentTab);
            }
        }
    }
}
