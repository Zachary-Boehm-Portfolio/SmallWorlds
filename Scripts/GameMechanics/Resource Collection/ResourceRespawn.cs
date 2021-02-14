using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceState
{
    LTree, // 0
    Lstump, // 1
    STree, // 2
    SStump, // 3
    Stone, // 4
    Coal, // 5
    Copper, // 6
    Iron, // 7
    Gold, // 8
    Other // last
}
public class ResourceRespawn : MonoBehaviour
{
    [Header("Stages of Object")]
    public ResourceState State;
    public ResourceState DefaultState;
    public int type; // 0 is wood and 1 is stone
    [Header("DataBase of Objects")]
    public TreeStages TStages;
    public StoneStages SStages;
    [Header("Current GameObject Spawned")]
    public GameObject currentStage;
    //for respawning
    [Header("Respawn")]
    public System.TimeSpan brokenTimestamp;
    public int RespawnRate; // factor of 45?

    private void Awake() {
        TStages = GameObject.Find("WorldLogic").GetComponentInChildren<TreeStages>();
        SStages = GameObject.Find("WorldLogic").GetComponentInChildren<StoneStages>();
        updatestage((int)State);
    }
    //when called from another script this will change the current object to it's next stage
    public void nextStage(){
        //change the current state
        switch((int)State)
        {
            case 0:
                State = (ResourceState) 1;
                updatestage((int)State);
                break;
            case 2:
                State = (ResourceState) 3;
                updatestage((int)State);
                break;
            case 5:
                State = (ResourceState) 4;
                updatestage((int)State);
                break;
            case 6:
                State = (ResourceState) 4;
                updatestage((int)State);
                break;
            case 7:
                State = (ResourceState) 4;
                updatestage((int)State);
                break;
            case 8:
                State = (ResourceState) 4;
                updatestage((int)State);
                break;
            default:
                State = (ResourceState) 9;
                updatestage((int)State);
                break;
        }
    }
    //will update what object is currently spawned in that location
    private void updatestage(int stage)
    {
        if(stage == 9)
        {
            GameObject temp;
            if(type == 1)
            {
                temp = Instantiate(SStages._stonestages[5], transform.position, Quaternion.identity);
            }
            else
            {
                temp = Instantiate(TStages._treestages[4], transform.position, Quaternion.identity);
            }
            Destroy(currentStage);
            temp.transform.parent = transform;
            currentStage = temp;
            GameEvents.current.onTimeChangeTriggerEnter += checkTime;
        }
        else
        {
            if(type == 0)
            {
                GameObject temp = Instantiate(TStages._treestages[stage], transform.position, Quaternion.identity);
                Destroy(currentStage);
                temp.transform.parent = transform;
                currentStage = temp;
            }else if (type == 1)
            {
                GameObject temp = Instantiate(SStages._stonestages[stage - 4], transform.position, Quaternion.identity);
                Destroy(currentStage);
                temp.transform.parent = transform;
                currentStage = temp;
            }
        }
    }
    //part of even system. when called it will see if enough time has passed for the object to respawn.
    public void checkTime(System.TimeSpan _time)
    {
        if(Mathf.Abs(TimeToSec(brokenTimestamp) - TimeToSec(_time)) > RespawnRate)
        {
            ResetObject();
            GameEvents.current.onTimeChangeTriggerEnter -= checkTime;
        }
    }
    //converts the System.TimeSpan 
     private int TimeToSec(System.TimeSpan _time)
    {
        int seconds = 0;
        seconds = ((_time.Hours * 60 * 60) + (_time.Minutes * 60) + _time.Seconds);
        return seconds;
    }
    public void ResetObject(){
        updatestage((int)DefaultState);
        State = DefaultState;
    }
}
