/*
	Project:    Energy Crisis
	
	Script:     Interactable
	Desc:       Handles trigger which activates player's interact UI
	
	Credits:	Brandon Carlisle
	
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {

	#region Variables
	
    public GameObject playerObj;
	PlayerInteract playerInteract;
    public MissionManager mm;
    public PlayerMovement playerMovement;
    public BatteryManager bm;
    public PlayerHealth playerHealth;
    public PlayerMelee playerMelee;
    //Renderer renderer;
    //Camera camera;
    public string text;
    public bool canTrigger = false;
    [SerializeField] public bool triggerToInteract = false; //No gui, just enter trigger
    public bool guiOn = false;
    public bool tellMM = false;
    //public bool visible = false;

    #endregion

    #region Unity Methods

    public void Start()
    {
        //renderer = this.gameObject.GetComponent<Renderer>();
    }

    public void Update()
    {
        //visible = renderer.isVisible;
        //if (canTrigger && !guiOn && renderer.isVisible) {
        //    enterInfo(playerInteract);
        //    guiOn = true;
        //}

        //Probably laggy
        //Vector3 cameraPos = camera.WorldToViewportPoint(transform.position);
        //visible = (cameraPos.z > 0 && cameraPos.x > 0 && cameraPos.x < 1 && cameraPos.y > 0 && cameraPos.y < 1);

        if (canTrigger && !guiOn) {
             {
                enterInfo(playerInteract);
                guiOn = true;
                return;
            }
        }
    }

    #endregion

    #region Methods
	
	private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {

            if (playerObj == null) {
                playerObj = other.gameObject;
                playerInteract = playerObj.GetComponent<PlayerInteract>();
                mm = playerObj.GetComponentInChildren<MissionManager>();
                bm = playerObj.GetComponentInChildren<BatteryManager>();
                playerMovement = playerObj.GetComponent<PlayerMovement>();
                playerHealth = playerObj.GetComponent<PlayerHealth>();
                playerMelee = playerObj.GetComponent<PlayerMelee>();
            }

            if (tellMM && mm) {
                mm.gotTrigger();
            }

            if (triggerToInteract) {
                interact();
                return;
            }
            
            canTrigger = true;
            //SceneManager.LoadScene("Alleyway");

        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player") && guiOn) {
            playerInteract.closeInteractGUI(this);
            canTrigger = false;
            guiOn = false;
        }
    }

    public virtual void enterInfo(PlayerInteract playerInteract) { 
        playerInteract.openInteractGUI(this);
    }

    public virtual void interact() { 
        if (tellMM && mm) {
            mm.gotInteract();
        }
    }

    public virtual void changeText(string inText) {
        text = inText;
    }

    #endregion
}