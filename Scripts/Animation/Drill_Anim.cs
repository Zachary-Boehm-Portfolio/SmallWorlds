using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drill_Anim : MonoBehaviour
{
    [Header("Drill Head")]
    public GameObject DrillHead;
    [Header("Animation Variables")]
    [SerializeField]public float rotation = 0f;
    public float speed;
    public float Clamp;
    public bool OnOff;
    private void Update()
    {
        if (OnOff)
        {
            rotation += speed * Time.deltaTime;
            rotation = Mathf.Clamp(rotation, -Clamp, Clamp);
            if (rotation == -Clamp || rotation == Clamp)
            {
                speed *= -1;
            }
            DrillHead.transform.localRotation = Quaternion.Euler(rotation, 90, 0);
        }
    }

    public void Toggle()
    {
        OnOff = !OnOff;
    }
}
