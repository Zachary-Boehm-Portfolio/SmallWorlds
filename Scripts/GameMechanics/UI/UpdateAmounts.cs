using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UpdateAmounts : MonoBehaviour
{
    [Header("Text amounts")]
    public TextMeshProUGUI[] Amounts;
    public ItemInfo.ResourceType type;
    [Header("Sliders")]
    public Slider[] Resource;
    [Header("ToolBelt Images")]
    public Image[] UnlockTools;
    // Start is called before the first frame update
    private void Start()
    {
        for (int i = 0; i < Resource.Length; i++)
        {
            Amounts[i].SetText(Resource[i].value.ToString() + "/" + Resource[i].maxValue.ToString());
        }
    }
    public int UpdateCrop(ItemInfo _item, int _amount)
    {
        if (type == ItemInfo.ResourceType.Crop)
        {
            if (UpdateResource((int)_item.cropName, _amount))
            {
                return (int)_item.cropName;
            }
            Debug.Log("Amount to be added or removed is out of bounds");
            return -1;
        }
        return -1;
    }
    public int UpdateOreWood(ItemInfo _item, int _amount)
    {
        if (type == ItemInfo.ResourceType.OreWood)
        {
            if (UpdateResource((int)_item.oreWoodName, _amount))
            {
                return (int)_item.oreWoodName;
            }
            Debug.Log("Amount to be added or removed is out of bounds");
            return -1;
        }
        return -1;
    }
    public int UpdateProduct(ItemInfo _item, int _amount)
    {
        if (type == ItemInfo.ResourceType.Product)
        {
            if (UpdateResource((int)_item.productName, _amount))
            {
                return (int)_item.productName;
            }
            Debug.Log("Amount to be added or removed is out of bounds");
            return -1;
        }
        return -1;
    }
    public int UpdateTool(ItemInfo _item)
    {
        if (type == ItemInfo.ResourceType.Belt)
        {
            if (UnlockTool((int)_item.toolName, true))
            {
                return (int)_item.toolName;
            }
            return -1;
        }
        Debug.Log("Not a tool");
        return -1;
    }
    //updates the resource amount by either adding to it or subtracting from it
    public bool UpdateResource(int _index, int _amount)
    {
        float total = Resource[_index].value + _amount;
        if(total < 0)
        {
            return false;
        }else if (total > Resource[_index].maxValue)
        {
            setResource(_index, (int)Resource[_index].maxValue);
            return true;
        }
        setResource(_index, (int)(Resource[_index].value + _amount));
        return true;
    }
    //sets the resource amount to whatever is passed in
    public bool setResource(int _index, int _amount)
    {
        Resource[_index].value = _amount;
        Amounts[_index].SetText(Resource[_index].value.ToString() + "/" + Resource[_index].maxValue.ToString());
        return true;
    }
    //will try to set the tool to unlock or lock 
    public bool UnlockTool(int _index, bool _unlock)
    {
        if (_index > -1 && _index < 3)
        {
            if(UnlockTools[_index].enabled == _unlock)
            {
                return false;
            }
            else
            {
                UnlockTools[_index].enabled = _unlock;
                return true;
            }
        }
        return false;
    }
    // Will load from the inventory save file and update the information
    public void LoadFromFile()
    {
        Inventory _inventory = SaveData.Load();
        switch ((int)type)
        {
            case 0:
                for (int i = 0; i < _inventory.Items[0].Length; i++)
                {
                    setResource(i, _inventory.Items[0][i][0]);
                    UpdateCapacity(i,  _inventory.Items[0][i][1]);
                }
                break;
            case 1:
                for (int i = 0; i < _inventory.Items[1].Length; i++)
                {
                    setResource(i, _inventory.Items[1][i][0]);
                    UpdateCapacity(i, _inventory.Items[1][i][1]);
                }
                break;
            case 2:
                for (int i = 0; i < _inventory.Items[2].Length; i++)
                {
                    setResource(i, _inventory.Items[2][i][0]);
                    UpdateCapacity(i, _inventory.Items[2][i][1]);
                }
                break;
            case 3:
                for (int i = 0; i < _inventory.Items[3].Length; i++)
                {
                    if (_inventory.Items[3][i][0] == 1)
                    {
                        UnlockTool(i, true);
                    }
                    else
                    {
                        UnlockTool(i, false);
                    }
                }
                break;
        }
    }
    //This function will update the capacity of a specific resource
    public void UpdateCapacity(int _index, int _capacity)
    {
        Resource[_index].maxValue = _capacity;
        Amounts[_index].SetText(Resource[_index].value.ToString() + "/" + Resource[_index].maxValue.ToString());
    }
}
