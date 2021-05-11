/*
	Project:	
	
	Script:		
	Desc:		
	
	Last Edit:	
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


    #region Methods
	
    //public override void enterInfo(PlayerInteract playerInteract) {
    //    playerInteract.openInteractGUI(this);
    //}

    public override void interact() {
        base.interact();

        if (warehouseDoor && base.playerMovement.secondSpawn) {
            Transform secondSpawnLoc = GameObject.FindGameObjectWithTag("SecondSpawn").transform;
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
            SceneManager.LoadScene(scene);
        }


        if (GetComponent<DoorAnimation>() != null) {
            GetComponent<DoorAnimation>().enabled = true;
        }

        this.enabled = false;
    }

    #endregion
}