using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MouseLook : MonoBehaviour
{
    [Header("This goes on the first-person camera")]
    [Space(10)]

    [Header("Player References")]
    public Transform playerBody;
    public BatteryClicker batteryClicker;

    [SerializeField] public Canvas pauseCanvas;

    public float mouseSensitivity = 100f;

    float xRotation = 0f;

    public bool paused = false;
    bool canMove = true;

    bool look = false;
    Vector3 lookPos;
    int speed = 10;

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
                pauseCanvas.enabled = true;
                batteryClicker.enabled = true;
                Time.timeScale = 0;
                return;
            } else {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                pauseCanvas.enabled = false;
                batteryClicker.enabled = false;
                Time.timeScale = 1;
            }
        }

        if (look) {
        Quaternion targetRotation = Quaternion.LookRotation(lookPos - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
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
        //transform.LookAt(lookPos);
        this.lookPos = lookPos;
        look = true;
    }

    public void holdMouse() {
        canMove = false;
    }

    public void releaseMouse() {
        canMove = true;
        look = false;
    }

    //The Restart button in the pause menu calls this
    public void restartScene() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
        //SaveData.instance?.ClearAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        SaveData.instance?.Load();
    }

    //The Exit button on the pause menu calls this
    public void exitToMenu() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1;
        SceneManager.LoadScene("TitleScreen");
    }
}
