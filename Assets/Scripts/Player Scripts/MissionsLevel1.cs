/*
	Project:    Energy Crisis
	
	Script:     MissionLevel1
	Desc:       The objectives for the tutorial level in the Warehouse
	
	Credits:	Brandon Carlisle
	
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class MissionsLevel1 : MissionManager {

	#region Variables

    [SerializeField] GameObject enemy_formelee;
    [SerializeField] GameObject enemy_forshoot;
    [SerializeField] GameObject battery1;
    [SerializeField] GameObject door;

    #endregion

    #region Unity Methods

	
    //Get one weapon and one battery
	public override void StartMission1() {

        objectiveGUI.SetText("Get a weapon and a battery");

        //if (weaponSwitcher.totalWeapons <= 0)
            currentMissions.Add(new Mission(Mission.MissionType.GetWeapon,Mission.CheckComponent.WeaponSwitcher,0,1)); //Get Weapon Mission
        //if (batteryManager.batteries.Count <= 0)
            currentMissions.Add(new Mission(Mission.MissionType.GetBattery,Mission.CheckComponent.BatteryManager,0,1)); //Get Battery Mission
    
    missionNumber = 1;
    missionComplete.RemoveListener(StartMission1);
    missionComplete.AddListener(StartMission2);
    getComponents();
    hasEvent = true;
    }

    

    //Mission 2
    void StartMission2() {
        objectiveGUI.SetText("The battery's dead! Go up to glowing enemies and hold F to melee kill and charge up");

        enemy_formelee.SetActive(true); //Enemy to melee

        currentMissions.Add(new Mission(Mission.MissionType.KillEnemy,Mission.CheckComponent.AllAttacks,0,1)); //Kill Enemy Mission

        missionNumber = 2;
        missionComplete.RemoveListener(StartMission2);
        missionComplete.AddListener(StartMission3);
        getComponents();
        hasEvent = true;
    }

    /*
    //Mission 3
    void StartMission3() {
        objectiveGUI.SetText("Press R to reload");

        currentMissions.Add(new Mission(Mission.MissionType.PressReload,Mission.CheckComponent.Gun,0,1)); //Reload Weapon Mission
        //currentMissions.Add(new Mission(Mission.MissionType.PressFire,Mission.CheckComponent.Gun,0,1)); //Fire Gun Mission

        missionNumber = 3;
        missionComplete.RemoveListener(StartMission3);
        missionComplete.AddListener(StartMission4);
        getComponents();
        hasEvent = true;
    }
    */

    //Mission 3
    void StartMission3() {
        objectiveGUI.SetText("Shoot and kill an enemy (R to reload, Left-Click to fire). The enemy will release an energy field that you can stand in to charge your batteries");

        battery1.SetActive(true);
        enemy_forshoot.SetActive(true); //Enemy to shoot

        currentMissions.Add(new Mission(Mission.MissionType.KillEnemy,Mission.CheckComponent.AllAttacks,0,1)); //Kill Enemy Mission

        missionNumber = 3;
        missionComplete.RemoveListener(StartMission3);
        missionComplete.AddListener(StartMission4);
        getComponents();
        hasEvent = true;
    }

    void StartMission4() {
        objectiveGUI.SetText("Charge up and go outside through front door when you're ready");

        door.GetComponent<Outline>().enabled = true;
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