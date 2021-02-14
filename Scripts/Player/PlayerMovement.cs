using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Controller Reference")]
    public CharacterController controller;
    
    [HideInInspector]public float speed = 3f;
    [Header("Movement")]
    public float WALKSPEED;
    public float runSpeed;
    public float jumpHeight = 3f;
    public bool canMove = true;
    [Header("Physics")]
    [SerializeField]public Vector3 velocity;
    public float gravity = -9.8f;
    public float groundDistance = 0.1f;
    [Header("Floor checks")]
    //checks for ground
    public Transform groundCheck;
    public LayerMask GroundMask;
    public bool isGrounded;
    private void Start()
    {
        speed = WALKSPEED;
    }
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, GroundMask);
        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        
        if(canMove)
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            Vector3 move = (transform.right * x + transform.forward * z);
            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed = runSpeed;
            }
            else
            {
                speed = WALKSPEED;
            }
            controller.Move(move * speed * Time.deltaTime);
            if(Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            }

        }
        velocity.y += gravity * Time.deltaTime;
        //Vertical Movement
        controller.Move(velocity * Time.deltaTime);
    }
}
