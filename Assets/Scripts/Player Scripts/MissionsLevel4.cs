/*
	Project:    Energy Crisis
	
	Script:     MissionLevel4
	Desc:       The objectives for the SECOND time in Alleyway, this time starting at the roof
	
	Credits:	Brandon Carlisle
	
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class MissionsLevel4 : MissionManager {

	#region Variables

    [SerializeField] EnemySpawner disableSpawner;
    [SerializeField] EnemySpawner spawner;
    [SerializeField] BoxCollider powerPoleInteractCollider;
    [SerializeField] Interactable powerPoleInteractable;
    [SerializeField] SphereCollider realCollider;
    [SerializeField] StealBattery stealBattery;
    [SerializeField] PowerPole powerPole;
    [SerializeField] GameObject redLight1;
    [SerializeField] GameObject redLight2;
    [SerializeField] GameObject roofBarrier;
    [SerializeField] Door farmDoor;
    private bool notTheseMissions = false;

    #endregion

    #region Unity Methods

    public override void checkSecondSpawn() {

        if (!PlayerMovement._instance.secondSpawn) {
            notTheseMissions = true;
            Destroy(this);
            return;
        }

        Debug.Log("MM recognizes Second Spawn");

        //MissionManager[] missionManagers = this.gameObject.GetComponents<MissionManager>();
        //Destroy(missionManagers[0]);
    }

	
    //Inspect the electrical box
	public override void StartMission1() {
        if (notTheseMissions)
            return;
        
        objectiveGUI.SetText("You don't have enough batteries to take them out. Look around for something");

        disableSpawner.enabled = false;
        spawner.enabled = true;
        spawner.spawn(20);
        
        currentMissions.Add(new Mission(Mission.MissionType.Trigger,Mission.CheckComponent.Interactable,0,1)); //Interact Mission
    
        missionNumber = 1;
        missionComplete.RemoveListener(StartMission1);
        missionComplete.AddListener(StartMission2);
        getComponents();
        hasEvent = true;
    }

    //Remove a battery and place it in the box
    void StartMission2() {
        objectiveGUI.SetText("If this power pole gets more power, it might blow");

        Destroy(powerPoleInteractable);
        Destroy(redLight1);
        Destroy(redLight2);
        Destroy(roofBarrier);
        //Destroy(powerPoleInteractCollider);

        //realCollider.enabled = true;
        stealBattery.enabled = true;
        base.interactables[1] = stealBattery; //Cheaty work-around
        
        currentMissions.Add(new Mission(Mission.MissionType.Interact,Mission.CheckComponent.Interactable,0,1)); //Interact Mission
    
        missionNumber = 2;
        missionComplete.RemoveListener(StartMission2);
        missionComplete.AddListener(StartMission3);
        getComponents();
        hasEvent = true;
    }

    

    //Kill them all
    void StartMission3() {
        objectiveGUI.SetText("That's helping! Take them out!");

        Destroy(stealBattery);
        Destroy(powerPoleInteractCollider);
        //realCollider.enabled = true;
        //powerPole.enabled = true;

        currentMissions.Add(new Mission(Mission.MissionType.WipeSpawner,Mission.CheckComponent.EnemySpawner,0,1));

        missionNumber = 3;
        missionComplete.RemoveListener(StartMission3);
        missionComplete.AddListener(StartMission4);
        getComponents();
        hasEvent = true;
    }

    void StartMission4() {
        objectiveGUI.SetText("That's all of them. Now let's go check out that farm...");

        farmDoor.enabled = true;

        missionNumber = 4;
        missionComplete.RemoveListener(StartMission4);

        //This will end with loading to a new scene
    }

    void NoMission() {
        objectiveGUI.SetText("No Mission");
    }


    #endregion
}