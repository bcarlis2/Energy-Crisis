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
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class MissionsLevel3 : MissionManager {

	#region Variables

    [SerializeField] EnemySpawner spawner;
    [SerializeField] GameObject door;

    #endregion

    #region Unity Methods

	
    //Open the door
	public override void StartMission1() {

        objectiveGUI.SetText("Go to the office");
        
        currentMissions.Add(new Mission(Mission.MissionType.Interact,Mission.CheckComponent.Interactable,0,1)); //Interact Mission
    
        missionNumber = 1;
        missionComplete.RemoveListener(StartMission1);
        missionComplete.AddListener(StartMission2);
        getComponents();
        hasEvent = true;
    }

    

    //Kill the enemies that just spawned
    void StartMission2() {
        objectiveGUI.SetText("Clear out the robots");

        spawner.spawn(4);

        //currentMissions.Add(new Mission(Mission.MissionType.KillEnemy,Mission.CheckComponent.AllAttacks,0,4)); //Kill Enemy Mission
        currentMissions.Add(new Mission(Mission.MissionType.WipeSpawner,Mission.CheckComponent.EnemySpawner,0,1));

        missionNumber = 2;
        missionComplete.RemoveListener(StartMission2);
        missionComplete.AddListener(StartMission3);
        getComponents();
        hasEvent = true;
    }

    //Find the Farm map
    void StartMission3() {
        objectiveGUI.SetText("Inspect the office");

        currentMissions.Add(new Mission(Mission.MissionType.Interact,Mission.CheckComponent.Interactable,0,1)); //Interact Mission

        missionNumber = 3;
        missionComplete.RemoveListener(StartMission3);
        missionComplete.AddListener(StartMission4);
        getComponents();
        hasEvent = true;
    }

    //Find the ladder
        void StartMission4() {
        objectiveGUI.SetText("Find a way out");

        door.GetComponent<Door>().enabled = true;

        missionNumber = 4;
        missionComplete.RemoveListener(StartMission4);

        //This will end with loading to a new scene
    }

    void NoMission() {
        objectiveGUI.SetText("No Mission");
    }


    #endregion
}