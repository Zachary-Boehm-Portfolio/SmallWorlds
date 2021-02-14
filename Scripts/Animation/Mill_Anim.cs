using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mill_Anim : MonoBehaviour
{
    [Header("Blades")]
    public GameObject Blades;
    [Header("Rotation Variables")]
    public float Speed;
    public bool OnOff;
    void Update()
    {
        if (OnOff)
        {
            Blades.transform.Rotate(new Vector3(0, 0, Speed * Time.deltaTime));
        }
        else
        {
            Blades.transform.localRotation = Quaternion.Euler(0, 90, 0);
        }
    }
    public void Toggle()
    {
        OnOff = !OnOff;
    }
}
