using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolEquip : MonoBehaviour
{
    [Header("Player Controller")]
    private BackpackController PlayerController;
    [SerializeField]private int CurrentTool = -1; // range of -1 to 2. -1: hand | 0: axe | 1: pickaxe | 2: shovel
    public GameObject currentTool;

    private void Start()
    {
        // grab the controller so the logic can tell if the player has that tool
        PlayerController = GameObject.Find("Player").GetComponentInChildren<BackpackController>();
        // start the player with nothing in their hand
        for (int i = 0; i < 3; i++)
        {
            transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }
    private void Update()
    {
        // Hand Equip
        if (Input.GetKeyDown(KeyCode.Alpha4) && CurrentTool != -1)
        {
            changeTool(-1);
        }
        // Axe Equip
        if (Input.GetKeyDown(KeyCode.Alpha1) && CurrentTool != 0)
        {
            if (PlayerController.Inventory.Items[3][0][0] == 1)
            {
                changeTool(0);
            }
        }
        // Pickaxe Equip
        else if (Input.GetKeyDown(KeyCode.Alpha2) && CurrentTool != 1)
        {
            if (PlayerController.Inventory.Items[3][1][0] == 1)
            {
                changeTool(1);
            }
        }
        // Shovel Equip
        else if (Input.GetKeyDown(KeyCode.Alpha3) && CurrentTool != 2)
        {
            if (PlayerController.Inventory.Items[3][2][0] == 1)
            {
                changeTool(2);
            }
        }
        // Xbox controller go to next tool to the right
        if (Input.GetButtonDown("ToolRight") && CurrentTool != 2)
        {
            bool nextTool = false;
            do
            {
                CurrentTool++;
                if(PlayerController.Inventory.Items[3][CurrentTool][0] == 1)
                {
                    nextTool = true;
                }
            } while (!nextTool && CurrentTool < 2);
            if(nextTool == true)
            {
                changeTool(CurrentTool);
            }
        }else if(Input.GetButtonDown("ToolRight") && CurrentTool == 2)
        {
            CurrentTool = -1;
            changeTool(CurrentTool);
        }
        // Xbox controller go to next tool to the left
        if (Input.GetButtonDown("ToolLeft") && CurrentTool != -1)
        {
            bool nextTool = false;
            CurrentTool--;
            while(CurrentTool > -1 && nextTool == false)
            {
                if(PlayerController.Inventory.Items[3][CurrentTool][0] == 1)
                {
                    nextTool = true;
                    break;
                }
                CurrentTool--;
            }
            changeTool(CurrentTool);
        }else if(Input.GetButtonDown("ToolLeft") && CurrentTool == -1)
        {
            CurrentTool = 2;
            changeTool(2);
        }
    }
    //will toggle the tool in hand to the one that is passed via child index
    public void changeTool(int _index)
    {
        
        CurrentTool = _index;
        //if current tool in hand is not null set it to not active
        if(currentTool != null)
        {
            currentTool.GetComponent<MeshRenderer>().enabled = false;
        }
        //if -1 then you want hand so just return
        if(_index == -1)
        {
            return;
        }
        // if index is within the bounds then put the tool that is to be in hand as active
        else if(_index < 3)
        {
            currentTool = transform.GetChild(_index).gameObject;
            currentTool.GetComponent<MeshRenderer>().enabled = true;
        }
    }
    public int GetCurrentTool()
    {
        return CurrentTool;
    }
}
