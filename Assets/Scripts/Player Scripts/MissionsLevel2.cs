/*
	Project:    Energy Crisis
	
	Script:     MissionLevel2
	Desc:       The objectives for first time entering the Alleyway
	
	Credits:	Brandon Carlisle
	
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class MissionsLevel2 : MissionManager {

	#region Variables

    [SerializeField] EnemySpawner realMob;
    PlayerMovement playerMovement;
    private bool notTheseMissions = false;

    #endregion

    #region Unity Methods

    public override void checkSecondSpawn() {

        if (PlayerMovement._instance.secondSpawn) {
            notTheseMissions = true;
            Destroy(this);
            return;
        }

        //MissionManager[] missionManagers = this.gameObject.GetComponents<MissionManager>();
        //Destroy(missionManagers[0]);
    }

	
    //Reach the "mob of enemies" trigger
	public override void StartMission1() {
        if (notTheseMissions)
            return;
        
        objectiveGUI.SetText("Explore and find supplies");
        
        currentMissions.Add(new Mission(Mission.MissionType.Trigger,Mission.CheckComponent.Interactable,0,1)); //Interact Mission
    
        missionNumber = 1;
        missionComplete.RemoveListener(StartMission1);
        missionComplete.AddListener(StartMission2);
        getComponents();
        hasEvent = true;
    }

    //Reach the "corner" trigger
    void StartMission2() {
        objectiveGUI.SetText("Explore and find supplies.");

        realMob.spawn(20);
        
        currentMissions.Add(new Mission(Mission.MissionType.Trigger,Mission.CheckComponent.Interactable,0,1)); //Interact Mission
    
        missionNumber = 1;
        missionComplete.RemoveListener(StartMission2);
        missionComplete.AddListener(StartMission3);
        getComponents();
        hasEvent = true;
    }

    

    //Reach the door
    void StartMission3() {
        objectiveGUI.SetText("There's too many of them. Quick, through the alley!");

        missionNumber = 3;
        missionComplete.RemoveListener(StartMission3);

        //This will end with loading to a new scene
    }

    void NoMission() {
        objectiveGUI.SetText("No Mission");
    }


    #endregion
}