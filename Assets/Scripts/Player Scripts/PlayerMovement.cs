using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Space(10)]
    [Header("Player References")]
    public CharacterController controller;
    public GameObject playerCanvas;
    [Tooltip("Empty object placed right underneath the player")]
    public Transform groundCheck;
    [Tooltip("The layer labeling what counts as the ground")]
    public LayerMask groundMask;

    public Transform secondSpawnLoc;
    public Vector3 secondSpawnLoc_Alleyway;
    public Vector3 secondSpawnLoc_Warehouse = new Vector3(188.309998f,1.5f,2.20000005f);
    public static PlayerMovement _instance;

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

    [Space(10)]
    [Header("Places player at different parts at load")]
    public bool secondSpawn = false;
    public bool changeMissions = true;


    Vector3 velocity;
    bool isGrounded;

    void Awake() {
        if (_instance != null && _instance != this) {
            _instance.transform.position = this.transform.position;      //Places the player at player spawn
            _instance.transform.rotation = this.transform.rotation;

            if (_instance.changeMissions) { //Take MissionHolder off of the GameObject and give it to the player
                Destroy(_instance.gameObject.GetComponentInChildren<MissionManager>().gameObject);
                MissionManager[] newMMHolders = this.gameObject.GetComponentsInChildren<MissionManager>(true);
                foreach(MissionManager newMMHolder in newMMHolders) {
                    newMMHolder.gameObject.transform.SetParent(_instance.gameObject.transform);
                    newMMHolder.gameObject.GetComponent<MissionManager>().resetReferences();
                    newMMHolder.enabled = true;
                }
            }

            Destroy(this.gameObject);
        } else {
            Debug.Log("Else");
            _instance = this;
        }
        DontDestroyOnLoad(this.gameObject);

        Debug.Log("Instance's secondSpawn = " + _instance.secondSpawn);
        Debug.Log("This secondSpawn = " + secondSpawn);

        if (_instance.secondSpawn) {
            secondSpawnLoc = GameObject.FindGameObjectWithTag("SecondSpawn").transform;
            //secondSpawnLoc = getSecondSpawn();
            Debug.Log("Spawn: Second Loc: " + secondSpawnLoc);
            _instance.transform.position = secondSpawnLoc.position; //Places the player at the door
            _instance.transform.rotation = secondSpawnLoc.rotation;
            //secondSpawn = false;
        }

    }

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

/*
    //Called by Door script
    public void setSpawn(string scene, bool sLoc) {
        //Debug.Log("Spawn: Setting " + SceneManager.GetSceneByName(scene) + " to " + sLoc);
        switch (scene) {
            case "SampleScene": //Warehouse
                secondSpawn_Warehouse = sLoc;
                Debug.Log("Spawn: Have set Warehouse to " + secondSpawn_Warehouse);
                break;
            case "Alleyway": //Alleyway
                secondSpawn_Alleyway = sLoc;
                break;
        }
    }
*/

    public Vector3 getSecondSpawn() {
        Debug.Log("Spawn Getting: " + SceneManager.GetActiveScene().name);

        switch (SceneManager.GetActiveScene().name) {
            case "SampleScene": //Warehouse
                Debug.Log("Spawn Getting Warehouse which is " + _instance.secondSpawnLoc_Warehouse);
                return _instance.secondSpawnLoc_Warehouse;
            case "Alleyway": //Alleyway
                return secondSpawnLoc_Alleyway;
            default:
                return Vector3.zero;
        }
    }

    public void toMenu() {
        SceneManager.LoadScene("TitleScreen");
    }

    /*
    bool whichSpawn() {
        Debug.Log("Spawn: Getting");
        switch (SceneManager.GetActiveScene().name) {
            case "SampleScene": //Warehouse
                Debug.Log("Spawn in Warehouse sLoc:" + secondSpawn_Warehouse);
                return secondSpawn_Warehouse;
            case "Alleyway": //Alleyway
            Debug.Log("Spawn in Alleyway sLoc:" + secondSpawn_Warehouse);
                return secondSpawn_Alleyway;
            default:
                return false;
        }
    }
    */
}
