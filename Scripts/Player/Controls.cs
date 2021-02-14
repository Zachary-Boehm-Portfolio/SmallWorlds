using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Controls : MonoBehaviour
{
    [Header("Inventories")]
    public GameObject PlayerBackPack;
    public BackpackController BackPack;
    public bool ToggleBackPack = true;
    [Header("Misc Objects")]
    public Image cursor;
    public GameObject PlantType;
    public Transform Production;
    [Header("DayNightCycle")]
    public DayNightCycle Cycle;
    public LayerMask mask;
    private void Start()
    {
        PlantType = GameObject.Find("PlantType");
        Physics.queriesHitTriggers = true;
        Production = transform.GetChild(0).GetChild(1).GetChild(0).GetChild(5);
    }
    void Update()
    {
        //Raycast interaction when the interact key is pressed
        if ((Input.GetButtonDown("Fire1")) && ToggleBackPack)
        {
            if (!PlantType.transform.GetChild(0).gameObject.activeSelf)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit Hit;
                if (Physics.Raycast(ray, out Hit, 1.5f, mask))
                {
                    Transform HitObject = Hit.transform;
                    switch (HitObject.tag)
                    {
                        case "Lantern":
                            HitObject.parent.GetComponent<LanternToggle>().Toggle();
                            break;
                        case "Mill":
                            HitObject.parent.GetComponent<Mill_Anim>().Toggle();
                            Debug.Log("Mill Open");
                            toggleUI(ToggleBackPack, 1);
                            ToggleBackPack = !ToggleBackPack;
                            break;
                        case "Drill":
                            HitObject.parent.GetComponent<Drill_Anim>().Toggle();
                            Debug.Log("Drill Open");
                            if(BackPack.UpdateFluid(1, 10))
                            {
                                Debug.Log("Oil Added");
                            }
                            break;
                        case "FarmPlot":
                            FarmController temp = HitObject.GetComponent<FarmController>();
                            if (temp.plantState())
                            {
                                temp.Harvest();
                            }
                            else
                            {
                                PlantType.GetComponent<PlotController>().Controller = temp;
                                PlantType.transform.GetChild(0).gameObject.SetActive(true);
                                ToggleCursor(true);
                            }
                            break;
                        case "Silo":
                            Debug.Log("Silo Open");
                            if (BackPack.UpdateCapacity(ItemInfo.ResourceType.OreWood, 0, 200))
                            {
                                Debug.Log("Capacity Increased");
                                BackPack.updateBackPackFill();
                            }
                            else
                            {
                                Debug.Log("Capacity Not Changed");
                            }
                            break;
                        case "Barn":
                            Debug.Log("Barn Open");
                            break;
                        case "Well":
                            Debug.Log("Selected Well");
                            if(BackPack.UpdateFluid(0, 10))
                            {
                                Debug.Log("Water Added");
                            }
                            break;
                        case "Pickup":
                            ItemHolder info = HitObject.gameObject.GetComponent<ItemHolder>();
                            if (BackPack.UpdateItem(info.Info, info.amountPerPickup))
                            {
                                Debug.Log("Item was picked up");
                                BackPack.updateBackPackFill();
                                Destroy(HitObject.gameObject);
                            }
                            else
                            {
                                Debug.Log("Failed to pick item up");
                            }

                            break;
                        case "Oven":
                            Debug.Log("Oven Open");
                            toggleUI(ToggleBackPack, 0);
                            ToggleBackPack = !ToggleBackPack;
                            break;
                        case "WaterBarrel":
                            BackPack.UpdateFluidCapacity(0, 100);
                            Debug.Log("You can now hold 100 more water");
                            break;
                        case "OilBarrell":  
                            BackPack.UpdateFluidCapacity(1, 100);
                            Debug.Log("You can now hold 100 more Oil");
                            break;
                    }
                }
            }else
            {
                ToggleCursor(false);
                PlantType.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
        //if (Input.GetButtonDown("Inventory"))
        //{
        //    if (!PlantType.transform.GetChild(0).gameObject.activeSelf)
        //    {
        //        toggleUI(ToggleBackPack, -1);
        //        ToggleBackPack = !ToggleBackPack;
        //    }
        //    else
        //    {
        //        PlantType.transform.GetChild(0).gameObject.SetActive(false);
        //    }
        //}
        //if (Input.GetButtonDown("Cancel"))
        //{
        //    BackPack.Clear();
        //    try
        //    {
        //        GetComponentInChildren<ToolEquip>().changeTool(-1);
        //    }
        //    catch (Exception e) { Debug.Log(e.StackTrace); }
        //}
    }
    //will toggle the ui to whatever state equal to the bool passed in.
    public void toggleUI(bool _state, int _products)
    {
        PlayerBackPack.SetActive(ToggleBackPack);
        GameObject Tools = transform.GetChild(0).GetChild(2).gameObject;
        GameObject curr = Tools.GetComponent<ToolEquip>().currentTool;
        Tools.GetComponent<ToolEquip>().enabled = !_state;
        if(curr != null)
        {
            curr.GetComponent<MeshRenderer>().enabled = !_state;
            //Tools.GetComponent<Animator>().SetBool("Active", false);
            //curr.GetComponent<BoxCollider>().enabled = false;
        }
        ToggleCursor(_state);
        GetComponent<PlayerMovement>().canMove = !_state;
        if (_products != -1)
        {
            switch (_products)
            {
                case 0:
                    Production.GetChild(0).gameObject.SetActive(true);
                    GetComponent<MenuControl>().openCrafting(0);
                    break;
                case 1:
                    GetComponent<MenuControl>().openCrafting(1);
                    Production.GetChild(1).gameObject.SetActive(true);
                    break;
            }
        }
        else
        {
            if(Production.GetChild(0).gameObject.activeSelf || Production.GetChild(1).gameObject.activeSelf)
            {
                GetComponent<MenuControl>().closeCrafting();
            }
            //Production.GetChild(0).gameObject.SetActive(false);
            //Production.GetChild(1).gameObject.SetActive(false);
        }
    }
    public void ToggleCursor(bool _state)
    {
        if (_state)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        Cursor.visible = _state;
        cursor.enabled = !_state;
        Camera.main.GetComponent<MouseLook>().enabled = !_state;
        GetComponent<PlayerMovement>().canMove = !_state;
    }
    public void Quit()
    {
        Application.Quit();
    }
}
