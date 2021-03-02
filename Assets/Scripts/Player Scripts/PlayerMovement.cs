using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Space(10)]
    [Header("Player References")]
    public CharacterController controller;
    [Tooltip("Empty object placed right underneath the player")]
    public Transform groundCheck;
    [Tooltip("The layer labeling what counts as the ground")]
    public LayerMask groundMask;

    [Space(10)]
    [Header("Movement values")]
    public float speed = 12f;
    public float sprintSpeed = 24f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float groundDistance = 0.4f;
    public float moveSpeed;

    [Space(10)]
    [Header("Can halt player movement at any time")]
    public bool canMove = true;
    public bool sprinting = false;


    Vector3 velocity;
    bool isGrounded;

    void Start()
    {
        canMove = true;
    }

    void Update()
    {
        if (!canMove)
            return; //TODO: Make more methods do this

        isGrounded = controller.isGrounded; //Should world and might be faster? //WTF does this comment mean??
        //isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask); //Maybe use this if we have problems with the ground

        if (isGrounded && velocity.y < 0) {
            velocity.y = -2f; //Smoother landing than velcoity being 0
        }

        float x = Input.GetAxis("Horizontal"); //Mouse X input
        float z = Input.GetAxis("Vertical"); //Mouse Y input

        Vector3 move = transform.right * x + transform.forward * z;

        if (Input.GetButton("Sprint") && isGrounded) {
            Debug.Log("Sprinting");
            moveSpeed = sprintSpeed;
        } else {
            moveSpeed = speed;
        }

        controller.Move(move * moveSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded) {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); //Physics!
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
