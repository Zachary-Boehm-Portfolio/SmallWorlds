using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceInfo : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private int AmountPerDrop;
    [SerializeField] private ItemInfo.OreWoodName Type;
    //when called will return the amount of wood the object drops everytime it needs to drop wood
    public int DropResource()
    {
        return AmountPerDrop;
    }
    //returns the current health of the object
    public float GetHealth()
    {
        return health;
    }
    //will return the type of Ore wood that it is... so this can be used for everything under that tab
    public ItemInfo.OreWoodName GetOreWood()
    {
        return Type;
    }
    //deals damage to the given object, returns -1 for no wood dropped and and value 0 or more for wood dropped
    public int Damage(float _damage)
    {
        if(health - _damage > 0)
        {
            health -= _damage;
            return -1;
        }
        else
        {
            health = 0;
            return DropResource();
        }
    }
}
