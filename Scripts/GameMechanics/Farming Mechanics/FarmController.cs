using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmController : MonoBehaviour
{
    [Header("Growth and Production")]
    private bool Grown = false, planted = false;
    public System.TimeSpan PlantTime;
    public int GrowTime;
    //start of new stuff
    public int reducedTime;
    public int normalTime;
    public int BonusMultiplier = 1;
    public int MaxRandBonus;
    public int waterNeeded;
    [SerializeField]private int maxBonus;
    private int minBonus = 0;
    //end of new stuff
    private ClockScript clock;
    //Spawn and change plot values
    [Header("Plot Stages and Current")]
    private int CropType = 0; // 0:carrot 1:melon 2:wheat 3:pumpkin 4:cabbage 5:sunflower
    public int ID;
    private FarmStages Stages;
    public GameObject Plot = null;
    public stageHolder Database;
    public bool finalState;
    private int CurrentStage = 0;
    private BackpackController BackPack;
    // Start is called before the first frame update
    private void Awake()
    {
        clock = GameObject.FindGameObjectWithTag("WorldLogic").GetComponent<DayNightCycle>().GameTime;
        Database = GameObject.Find("StagesHolder").GetComponent<stageHolder>();
        BackPack = GameObject.Find("Player").GetComponentInChildren<BackpackController>();
    }
    void Start()
    {

        Inventory temp = SaveData.Load();
        CropType = temp.PlotStates[ID][0];
        Stages = Database.CropDatabase[temp.PlotStates[ID][0]];
        CurrentStage = temp.PlotStates[ID][1];
        updateState(CurrentStage); // add in logic to pull from save file and tell if a plot is at what stage
        if (CurrentStage > 0)
        {
            planted = true;
            GameEvents.current.onTimeChangeTriggerEnter += OnTimeChange;
        }
        if (CurrentStage == Stages.Stages.Length - 1)
        {
            finalState = true;
            Grown = true;
        }
    }

    private void OnTimeChange(System.TimeSpan _time)
    {
        if (Grown == false && planted == true)
        {
            if (Mathf.Abs(TimeToSec(PlantTime) - TimeToSec(_time)) >= GrowTime)
            {
                if (finalState)
                {
                    Grown = true;
                }
                else
                {
                    updateState(CurrentStage + 1);
                    CurrentStage++;
                    if (CurrentStage == Stages.Stages.Length - 1)
                    {
                        finalState = true;
                        Grown = true;
                    }
                    PlantTime = clock.clock.GetTime();
                }
            }
        }
        else
        {
            GameEvents.current.onTimeChangeTriggerEnter -= OnTimeChange;
        }
    }
    public void Plant(int _PlotType)
    {
        if (Grown == false)
        {
            Stages = Database.CropDatabase[_PlotType];
            BackpackController Backpack = GameObject.Find("Player").GetComponentInChildren<BackpackController>();
            if(Backpack.UpdateItem((ScriptableObject.CreateInstance("ItemInfo") as ItemInfo).init(ItemInfo.ResourceType.Crop, (ItemInfo.CropName)Stages.ID, ItemInfo.OreWoodName.Other, ItemInfo.ProductName.Other, ItemInfo.ToolName.Other), -9))
            {
                Debug.Log("Planted the crop");
                PlantTime = clock.clock.GetTime();
                updateState(1);
                Grown = false;
                planted = true;
                GameEvents.current.onTimeChangeTriggerEnter += OnTimeChange;
                CropType = _PlotType;
                Backpack.updateState(ID, 1, _PlotType);
                //start of water effects
                if(Backpack.UpdateFluid(0, -waterNeeded))
                {
                    GrowTime = reducedTime;
                    maxBonus = MaxRandBonus;
                }else
                {
                    GrowTime = normalTime;
                }
            }
            else
            {
                Debug.LogError("Not Enough Items to plant here");
            }
        }
    }
    public void Harvest()
    {
        if (Grown == true && finalState == true)
        {
            int random = Random.Range(minBonus, maxBonus);
            BackpackController Backpack = GameObject.Find("Player").GetComponentInChildren<BackpackController>();
            if (Backpack.UpdateItem((ScriptableObject.CreateInstance("ItemInfo") as ItemInfo).init(ItemInfo.ResourceType.Crop, (ItemInfo.CropName)Stages.ID, ItemInfo.OreWoodName.Other, ItemInfo.ProductName.Other, ItemInfo.ToolName.Other), (9 + random)*BonusMultiplier))
            {
                Debug.Log("plant has been harvested");
                updateState(0);
                planted = false;
                finalState = false;
                Grown = false;
                CurrentStage = 0;
                Backpack.updateState(ID, 0, CropType);
                BonusMultiplier = 1;
            }
            else
            {
                Debug.LogError("Your inventory for this item is not large enough to harvest");
            }
            
        }
    }
    public bool plantState()
    {
        return planted;
    }
    public bool growthState()
    {
        return Grown;
    }
    private int TimeToSec(System.TimeSpan _time)
    {
        int seconds = 0;
        seconds = ((_time.Hours * 60 * 60) + (_time.Minutes * 60) + _time.Seconds);
        return seconds;
    }
    public void updateState(int _index)
    {
        GameObject temp = Instantiate(Stages.Stages[_index], transform);
        Destroy(Plot);
        Plot = temp;
        Plot.transform.parent = transform;
        BackPack.updateState(ID, _index, CropType);
    }
}
