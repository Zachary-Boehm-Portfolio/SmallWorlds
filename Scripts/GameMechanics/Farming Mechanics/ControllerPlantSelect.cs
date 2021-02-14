using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerPlantSelect : MonoBehaviour
{
    [SerializeField]int selected = 0; // min = 0 | max = 5
    private bool m_isAxisInUse6 = false;
    private bool m_isAxisInUseTWO = false;
    GameObject currentChild;
    private void Start()
    {
        currentChild = transform.GetChild(0).gameObject;
        selected = 0;
    }
    //movement using the controller
    void FixedUpdate()
    {
        //Horizontal
        if (Input.GetAxis("Axis 6") != 0 && !m_isAxisInUse6)
        {
            m_isAxisInUse6 = true;
            if (Input.GetAxisRaw("Axis 6") > 0)
            {
                //edge cases
                if(selected != 5 && selected != 2)
                {
                    selected++;
                    updateSelectedChild(selected);
                }
            }
            else if(Input.GetAxisRaw("Axis 6") < 0)
            {
                //edge case
                if (selected != 0 && selected != 3)
                {
                    selected--;
                    updateSelectedChild(selected);
                }
            }
        }
        if(Input.GetAxis("Axis 6") == 0)
        {
            m_isAxisInUse6 = false;
        }
        //Vertical
        if (Input.GetAxis("Axis 7") != 0 && !m_isAxisInUseTWO)
        {
            m_isAxisInUseTWO = true;
            if (Input.GetAxisRaw("Axis 7") > 0)
            {
                //edge case
                if (selected < 3)
                {
                    selected += 3;
                    updateSelectedChild(selected);
                }
            }
            else if (Input.GetAxisRaw("Axis 7") < 0)
            {
                //edge case
                if (selected > 2 && selected < 6)
                {
                    selected -= 3;
                    updateSelectedChild(selected);
                }
            }
        }
        if (Input.GetAxis("Axis 7") == 0)
        {
            m_isAxisInUseTWO = false;
        }
        if (Input.GetButtonDown("Submit"))
        {
            transform.parent.gameObject.GetComponent<PlotController>().CropType(selected);
            selected = 0;
            updateSelectedChild(0);
        }
    }
    public void updateSelectedChild(int _selected)
    {
        if(currentChild != null)
        {
            currentChild.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
        currentChild = transform.GetChild(_selected).gameObject;
        currentChild.GetComponent<Image>().color = new Color(1, 1, 1, .13f);
    }
}
