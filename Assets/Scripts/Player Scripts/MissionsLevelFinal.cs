﻿/*
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

public class MissionsLevelFinal : MissionManager {

	#region Variables

    [SerializeField] EnemySpawner[] spawners;
    [SerializeField] CinemaCam cineCam;

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

    //Put the big battery in the generator
    void StartMission2() {
        objectiveGUI.SetText("Fight the horde and charge that battery! Then put it in the generator");
        
        currentMissions.Add(new Mission(Mission.MissionType.Interact,Mission.CheckComponent.Interactable,0,1)); //Interact Mission

        foreach(EnemySpawner spawner in spawners) {
            spawner.spawnInfinite(1); //How many should be active at any given moment
        }

        missionNumber = 2;
        missionComplete.RemoveListener(StartMission2);
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

        currentMissions.Add(new Mission(Mission.MissionType.WipeSpawner,Mission.CheckComponent.EnemySpawner,0,spawners.Length));

        missionNumber = 3;
        missionComplete.RemoveListener(StartMission3);
        missionComplete.AddListener(StartMission4);
        getComponents();
        hasEvent = true;
    }

    void StartMission4() {
        objectiveGUI.SetText("YOU DID IT");

        missionNumber = 4;
        missionComplete.RemoveListener(StartMission4);

        cineCam.enabled = true; //Angle1


        //This will end with loading to a new scene
    }

    void NoMission() {
        objectiveGUI.SetText("No Mission");
    }


    #endregion
}