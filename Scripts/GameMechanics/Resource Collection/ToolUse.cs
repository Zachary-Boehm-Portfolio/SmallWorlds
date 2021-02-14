using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SocialPlatforms;

public class ToolUse : MonoBehaviour
{
    [SerializeField] private float ToolDamage;
    private BackpackController BackPack;
    //TODO: Add the animation code
    //TODO: Add the detection for wood and stone tags and the logic for hitting the object
    private void Start()
    {
        BackPack = GameObject.Find("Player").GetComponentInChildren<BackpackController>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (tag == collision.gameObject.tag)
        {
            int droppedResource = -1;
            ResourceInfo info = null;
            try{
                info = collision.gameObject.GetComponent<ResourceInfo>();
                droppedResource = info.Damage(ToolDamage);
            }catch(Exception e){}
            
            switch (tag)
            {
                case "Wood":
                    if (droppedResource != -1)
                    {
                        BackPack.UpdateItem((ScriptableObject.CreateInstance("ItemInfo") as ItemInfo).init(ItemInfo.ResourceType.OreWood, ItemInfo.CropName.Other, info.GetOreWood(), ItemInfo.ProductName.Other, ItemInfo.ToolName.Other), droppedResource);
                        collision.transform.parent.gameObject.GetComponent<ResourceRespawn>().nextStage();
                        int rand = UnityEngine.Random.Range(1, 100);
                        if(rand >= 90)
                        {
                            //hard coded oil amount with a random chance of happening
                            BackPack.UpdateFluid(1, 20);
                        }
                    }
                    break;
                case "Rock":
                    //logic for hitting a rock with pickaxe here.
                    if(droppedResource != -1)
                    {
                        BackPack.UpdateItem((ScriptableObject.CreateInstance("ItemInfo") as ItemInfo).init(ItemInfo.ResourceType.OreWood, ItemInfo.CropName.Other, info.GetOreWood(), ItemInfo.ProductName.Other, ItemInfo.ToolName.Other), droppedResource);
                        collision.transform.parent.gameObject.GetComponent<ResourceRespawn>().nextStage();
                    }
                    break;
                case "FarmPlot":
                    Debug.Log("colliding with farmplot");
                    FarmController farm = collision.gameObject.GetComponent<FarmController>();
                    farm.BonusMultiplier = 2;
                    farm.Harvest();
                    break;
            }
            GetComponent<BoxCollider>().enabled = false;
        }
    }
    
}
