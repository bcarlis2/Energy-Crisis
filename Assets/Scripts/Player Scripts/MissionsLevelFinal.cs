/*
	Project:    Energy Crisis
	
	Script:     MissionLevelFinal
	Desc:       The objectives for the final level on the Farm
	
	Credits:	Brandon Carlisle
	
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class MissionsLevelFinal : MissionManager {

	#region Variables

    [SerializeField] EnemySpawner[] spawners;
    [SerializeField] CinemaCam cineCam;
    [SerializeField] Battery carBattery;
    [SerializeField] Outline genOutline;

    #endregion

    #region Unity Methods

	
    //Find the big battery
	public override void StartMission1() {
        objectiveGUI.SetText("Find a way to turn the power on");
        
        currentMissions.Add(new Mission(Mission.MissionType.Interact,Mission.CheckComponent.Interactable,0,1)); //Interact Mission
    
        missionNumber = 1;
        missionComplete.RemoveListener(StartMission1);
        missionComplete.AddListener(StartMission2);
        getComponents();
        hasEvent = true;
    }

    void StartMission2() {
        objectiveGUI.SetText("Fight the horde and charge that car battery!");
        
        carBattery.mm = this;
        currentMissions.Add(new Mission(Mission.MissionType.Charge,Mission.CheckComponent.Battery,0,1)); //Charge Mission

        foreach(EnemySpawner spawner in spawners) {
            spawner.spawnInfinite(1); //How many should be active at any given moment
        }

        missionNumber = 2;
        missionComplete.RemoveListener(StartMission2);
        missionComplete.AddListener(StartMission22);
        getComponents();
        hasEvent = true;
    }

    //Put the big battery in the generator
    void StartMission22() {
        objectiveGUI.SetText("Put the battery in the generator");
        
        genOutline.enabled = true;
        currentMissions.Add(new Mission(Mission.MissionType.Interact,Mission.CheckComponent.Interactable,0,1)); //Interact Mission

        missionNumber = 22;
        missionComplete.RemoveListener(StartMission22);
        missionComplete.AddListener(StartMission3);
        getComponents();
        hasEvent = true;
    }

    

    //Kill them rest
    void StartMission3() {
        objectiveGUI.SetText("The generator is attracting more. Take out the rest of them!");

        foreach(EnemySpawner spawner in spawners) {
            spawner.stopSpawnInfinite();
            spawner.spawn(1);
        }
        
        genOutline.enabled = false;
        AudioManager.instance?.playGenerator();
        currentMissions.Add(new Mission(Mission.MissionType.WipeSpawner,Mission.CheckComponent.EnemySpawner,0,spawners.Length));

        missionNumber = 3;
        missionComplete.RemoveListener(StartMission3);
        missionComplete.AddListener(StartMission4);
        getComponents();
        hasEvent = true;
    }

    void StartMission4() {
        objectiveGUI.SetText("You did it! You finally have a safe haven");

        missionNumber = 4;
        missionComplete.RemoveListener(StartMission4);

        PlayerMovement._instance.canMove = false; //Stop player
        Invoke(nameof(cutscene),5f);

        //cineCam.enabled = true; //Angle1


        //This will end with loading to a new scene
    }

    void cutscene() {
        AudioManager.instance?.stopGenerator();
        cineCam.enabled = true;
    }

    void NoMission() {
        objectiveGUI.SetText("No Mission");
    }


    #endregion
}