using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [Header("Mouse Sensitivity")]
    public float moustSensitivity = 100f;
    [Header("Transform of Player")]
    public Transform PlayerBody;
   
    private float xRotation = 0f;//Rotation from mouseY
    void Start()
    {
        //Puts the cursor in the center, hides it, and locks it.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
        //Get mouse input
        float mouseX = Input.GetAxisRaw("Look Horizontal") * moustSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Look Vertical") * moustSensitivity * Time.deltaTime;
        //sets the rotation for the look up and down
        xRotation -= mouseY;
        //clamp that keeps you from looking more than 180 degrees/ more than straight up or down
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        // Rotates the player based on the x movement of the mouse
        PlayerBody.Rotate(Vector3.up * mouseX);
        // Looks up and downs based on the Y input and the clamp
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
