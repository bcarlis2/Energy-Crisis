using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [Header("This goes on the first-person camera")]
    [Space(10)]

    [Header("Player References")]
    public Transform playerBody;

    public float mouseSensitivity = 100f;

    float xRotation = 0f;

    bool paused = false;
    bool canMove = true;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {

        if (Input.GetButtonDown("Cancel")) {
            paused = !paused;

            if (paused) {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                return;
            } else {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        if (paused || !canMove)
            return;
        
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); //Can look up, look down

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    public void playerLookAt(Vector3 lookPos) { //May break everything
        transform.LookAt(lookPos);
    }

    public void holdMouse() {
        canMove = false;
    }

    public void releaseMouse() {
        canMove = true;
    }
}
