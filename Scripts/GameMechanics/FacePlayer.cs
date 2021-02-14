using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    void Update()
    {
        try
        {
            transform.LookAt(GameObject.FindGameObjectWithTag("Player").transform);
        }
        catch (Exception e) { }
    }
}
