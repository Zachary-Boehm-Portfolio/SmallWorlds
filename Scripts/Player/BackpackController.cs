using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackpackController : MonoBehaviour
{
    [Header("Menu's")]
    public Image BackpackUI;
    public GameObject[] Menu;
    public Inventory Inventory;
    public DisplayAddedAmount DisplayText;
    //menu and tab currently selected
    private GameObject currentMenu;
    private Transform Tabs;
    private Transform currentTab;
    //when the application starts up it loads the savefile and loads the inventory
    private void Start()
    {
        Tabs = transform.GetChild(0).GetChild(1).GetChild(0);
        if (SaveData.Load() != null)
        {
            Inventory = SaveData.Load();
            LoadFile();
        }
        else
        {
            Inventory = new Inventory();
            Debug.Log("Created new Inventory");
        }
        updateBackPackFill();
        currentMenu = transform.GetChild(0).GetChild(1).gameObject;
        currentTab = Tabs.GetChild(0);
    }
    //saves inventory when the game is closed
    private void OnApplicationQuit()
    {
        SaveData.Save(Inventory);
        Inventory.printInventory();
    }
    //will toggle if a certain menu is open or not
    public void ToggleMenu(int _index)
    {
        if(_index == 0)
        {
            Tabs.GetChild(1).GetChild(1).gameObject.SetActive(false);
            Menu[2].SetActive(false);
            Menu[0].SetActive(true);
            Menu[1].SetActive(true);
            currentTab = Tabs.GetChild(0);
            currentTab.GetChild(1).gameObject.SetActive(true);
        }
        if (_index == 1)
        {
            Tabs.GetChild(0).GetChild(1).gameObject.SetActive(false);
            Menu[0].SetActive(false);
            Menu[1].SetActive(false);
            Menu[2].SetActive(true);
            currentTab = Tabs.GetChild(1);
            currentTab.GetChild(1).gameObject.SetActive(true);
        }
    }
    //This method will check the amount of the item and see if there is enough for crafting or enough space in general and return a true or false
    public bool CheckItemFill(int RType, int Res, int _compareAmount)
    {
        int Amount = Inventory.Items[RType][Res][0];
        if(Amount - _compareAmount < 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    //will pass the item into the menu's/inventory and try to add it and will return the position array wise for where it was placed
    public bool UpdateItem(ItemInfo _item, int _amount)
    {
        switch ((int)_item.Type)
        {
            case 0:
                int temp = Menu[0].GetComponent<UpdateAmounts>().UpdateCrop(_item, _amount);
                if (temp >= 0)
                {
                    int newAmount = (int)Menu[0].GetComponent<UpdateAmounts>().Resource[temp].value - Inventory.Items[0][temp][0];
                    Inventory.Items[0][temp][0] += newAmount;
                    Inventory.total += newAmount;
                    updateBackPackFill();
                    DisplayText.DisplayAmount(newAmount);
                    return true;
                }
                else
                {
                    return false;
                }

            case 1:
                temp = Menu[1].GetComponent<UpdateAmounts>().UpdateOreWood(_item, _amount);
                if (temp >= 0)
                {
                    int newAmount = (int)Menu[1].GetComponent<UpdateAmounts>().Resource[temp].value - Inventory.Items[1][temp][0];
                    Inventory.Items[1][temp][0] += newAmount;
                    Inventory.total += newAmount;
                    updateBackPackFill();
                    DisplayText.DisplayAmount(newAmount);
                    return true;
                }
                else
                {
                    return false;
                }
            case 2:
                temp = Menu[2].GetComponent<UpdateAmounts>().UpdateProduct(_item, _amount);
                if (temp >= 0)
                {
                    int newAmount = (int)Menu[2].GetComponent<UpdateAmounts>().Resource[temp].value - Inventory.Items[2][temp][0];
                    Inventory.Items[2][temp][0] += newAmount;
                    Inventory.total += newAmount;
                    updateBackPackFill();
                    DisplayText.DisplayAmount(newAmount);
                    return true;
                }
                else
                {
                    return false;
                }
            case 3:
                temp = Menu[3].GetComponent<UpdateAmounts>().UpdateTool(_item);
                if (temp >= 0)
                {
                    Inventory.Items[3][temp][0] = 1;
                    return true;
                }
                else
                {
                    return false;
                }
            case 4:
                return false;
        }
        return false;
    }
    //will pass the fluid to the fluid portial of inventory display and will return if it can add fluids to inventory or remove fluids from inventory
    public bool UpdateFluid(int _type, int _amount)
    {
        // Water      Oil
        // 0 0 0 4 | 0 0 0 5
        Transform tabs = transform.GetChild(0);
        if(_type == 0) // type 0 == water
        {
            //check of the amount can be either taken or added to the current inventory
            if(CheckFill(0, _amount))
            {
                //if it works then it will update the UI then the Backend inventory
                tabs.GetChild(6).gameObject.GetComponent<Slider>().value += _amount;
                Inventory.Fluids[_type][0] += _amount;
                DisplayText.DisplayAmount(_amount);
                return true;
            }
            return false;
        }else if(_type == 1) // type 1 == oil
        {
             if(CheckFill(1, _amount))
            {
                tabs.GetChild(7).gameObject.GetComponent<Slider>().value += _amount;
                Inventory.Fluids[_type][0] += _amount;
                DisplayText.DisplayAmount(_amount);
                return true;
            }
            return false;
        }
        Debug.Log("Incorrect Fluid Type");
        return false;
    }
    //this method will return the amount of fluid that is currently available of either water or oil
    public bool CheckFluidFill(int _type, int _amount)
    {
        int curr = Inventory.Fluids[_type][0];
        if(curr - _amount < 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    //this method will check to see if the amount being taken or added is valid for both water and oil
    public bool CheckFill(int _type, int _amount)
    {
        int updatedValue = Inventory.Fluids[_type][0] + _amount;
        if(updatedValue > 0 && updatedValue <= Inventory.Fluids[_type][1])
        {
            return true;
        }else
        { 
            return false;
        }
    }
    //updates the specified fluids capacity
    public void UpdateFluidCapacity(int _type, int _capacity)
    {
        Inventory.Fluids[_type][1] += _capacity;
        transform.GetChild(0).GetChild(_type + 6).gameObject.GetComponent<Slider>().maxValue += _capacity;
    }
    // will update the values on the display part of the inventory from the save file
    public void LoadFile()
    {
        for (int i = 0; i < Menu.Length; i++)
        {
            Menu[i].GetComponent<UpdateAmounts>().LoadFromFile();
        }
        Transform tabs = transform.GetChild(0);
        tabs.GetChild(6).gameObject.GetComponent<Slider>().value = Inventory.Fluids[0][0];
        tabs.GetChild(6).gameObject.GetComponent<Slider>().maxValue = Inventory.Fluids[0][1];
        tabs.GetChild(7).gameObject.GetComponent<Slider>().value = Inventory.Fluids[1][0];
        tabs.GetChild(7).gameObject.GetComponent<Slider>().maxValue = Inventory.Fluids[1][1];
    }
    // public method to clear the inventory both ingame and in the save file. Does not update the Display of the inventory.
    [ContextMenu("Clear")]
    public void Clear()
    {
        Inventory = new Inventory();
        SaveData.Save(Inventory);
        LoadFile();
        updateBackPackFill();
    }
    //This method will update the maximum value of a certain resource to whatever is paced in.
    public bool UpdateCapacity(ItemInfo.ResourceType Type, int _index, int _capacity) // the index is what resource is being changed
    {
        switch ((int)Type)
        {
            case 0:
                Menu[0].GetComponent<UpdateAmounts>().UpdateCapacity(_index, _capacity);
                Inventory.Items[0][_index][1] = _capacity;
                Inventory.max += _capacity;
                return true;

            case 1:
                Menu[1].GetComponent<UpdateAmounts>().UpdateCapacity(_index, _capacity);
                Inventory.max += _capacity;
                Inventory.Items[1][_index][1] = _capacity;
                return true;
            case 2:
                Menu[2].GetComponent<UpdateAmounts>().UpdateCapacity(_index, _capacity);
                Inventory.Items[2][_index][1] = _capacity;
                Inventory.max += _capacity;
                return true;
        }
        return false;
    }
    public void updateBackPackFill()
    {
        if (Inventory.total > 0)
        {
            BackpackUI.fillAmount = Inventory.total / Inventory.max;
        }
        else
        {
            BackpackUI.fillAmount = 0;
        }
    }
    public void updateState(int _id, int _state, int _cropType)
    {
        Inventory.PlotStates[_id][0] = _cropType;
        Inventory.PlotStates[_id][1] = _state;
    }
}
