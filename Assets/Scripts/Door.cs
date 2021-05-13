/*
	Project:    Energy Crisis
	
	Script:     Door
	Desc:       Interactable object that can open a door with an animation and load a specified scene
	
	Credits:	Brandon Carlisle
	
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : Interactable {

	#region Variables
	
    [SerializeField] public bool loadScene = false;
    public string scene = "Alleyway";
    public bool secondLoc = false;
    public bool changeMissions = true;
    [SerializeField] public bool warehouseDoor;

    #endregion

    public void Start()
    {
        if (warehouseDoor && PlayerMovement._instance.secondSpawn) {
            base.changeText("Go To Roof");
            secondLoc = true;
        }
        //renderer = this.gameObject.GetComponent<Renderer>();
    }

    #region Methods
	
    //public override void enterInfo(PlayerInteract playerInteract) {
    //    playerInteract.openInteractGUI(this);
    //}

    public override void interact() {
        base.interact();

        if (warehouseDoor && PlayerMovement._instance.secondSpawn) {
            Debug.Log("Taking the player to the roof");
            //Transform secondSpawnLoc = GameObject.FindGameObjectWithTag("SecondSpawn").transform;
            Transform secondSpawnLoc = PlayerMovement._instance.secondSpawnLoc;
            //Debug.Log("SecondSpawnLoc: " + secondSpawnLoc.position);
            base.playerMovement.transform.position = secondSpawnLoc.position; //Places the player at the roof
            base.playerMovement.transform.rotation = secondSpawnLoc.rotation;
            return;
            //secondSpawn = false;
        }

        if (loadScene) {
            SaveData.instance?.Save();

            //base.playerMovement.setSpawn(scene,secondLoc);
            base.playerMovement.secondSpawn = secondLoc;
            base.playerMovement.changeMissions = changeMissions;
            base.playerMelee.stopMelee(); //Stops melee UI from staying on screen
            SceneManager.LoadScene(scene);
        }


        if (GetComponent<DoorAnimation>() != null) {
            GetComponent<DoorAnimation>().enabled = true;
        }

        this.enabled = false;
    }

    #endregion
}