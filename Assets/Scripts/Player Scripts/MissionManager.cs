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

public class MissionManager : MonoBehaviour {

	#region Variables

    [System.Serializable]
    public class Mission {
        public enum MissionType {GetWeapon,GetBattery,MeleeEnemy,PressReload,PressFire}
        public enum CheckComponent {WeaponSwitcher,BatteryManager,PlayerMelee,Gun}

        public MissionType missionType;
        public CheckComponent toCheck;
        public int value; //Starting value for what needs to be increased
        public int goalValue; //End value needed to complete mission

        public Mission(MissionType type, CheckComponent check, int inV, int goalV) {
            missionType = type;
            toCheck = check;
            value = inV;
            goalValue = goalV;
        }
    }

    [SerializeField] WeaponSwitcher weaponSwitcher;
    [SerializeField] BatteryManager batteryManager;
    [SerializeField] PlayerMelee playerMelee;
    [SerializeField] Gun gun;
    [SerializeField] TextMeshProUGUI objectiveGUI;
    [SerializeField] EnemySpawner spawner1;
    [SerializeField] GameObject testEnemy;
    [SerializeField] GameObject door;
    [SerializeField] public List<Mission> currentMissions = new List<Mission>();
    bool checkWeaponSwitcher = false;
    bool checkBatteryManager = false;
    bool checkPlayerMelee = false;
    bool checkGun = false;
    int missionNumber = 0;
    UnityEvent missionComplete = new UnityEvent();
    bool hasEvent = false;

    #endregion

    #region Unity Methods

    public void Start()
    {
        StartMission1();

    }

    public void Update()
    {
        //Mission Complete, move on to next mission
        if (currentMissions.Count == 0 && hasEvent) {
            Debug.Log("Mission #" + missionNumber + "Complete!");
            hasEvent = false;
            missionComplete.Invoke();
        }

        //Updates which components should report back here
        bool keepWeaponSwitcher = false;
        bool keepBatteryManager = false;
        bool keepPlayerMelee = false;
        bool keepGun = false;

        foreach (Mission mission in currentMissions) {
            switch (mission.toCheck) {
                case Mission.CheckComponent.WeaponSwitcher:
                    keepWeaponSwitcher = true;
                    break;
                case Mission.CheckComponent.BatteryManager:
                    keepBatteryManager = true;
                    break;
                case Mission.CheckComponent.PlayerMelee:
                    keepPlayerMelee = true;
                    break;
                case Mission.CheckComponent.Gun:
                    keepGun = true;
                    break;
            }
        }
        if (checkWeaponSwitcher != keepWeaponSwitcher) {
            weaponSwitcher.tellMM = keepWeaponSwitcher;
            checkWeaponSwitcher = keepWeaponSwitcher;
        }
        if (checkBatteryManager != keepBatteryManager) {
            batteryManager.tellMM = keepBatteryManager;
            checkBatteryManager = keepBatteryManager;
        }

        if (checkPlayerMelee != keepPlayerMelee) {
            playerMelee.tellMM = keepPlayerMelee;
            checkPlayerMelee = keepPlayerMelee;
        }

        if (checkGun != keepGun) {
            gun.tellMM = keepGun;
            checkGun = keepGun;
        }
    }

    #endregion

    #region Methods

    //Called by WeaponSwitcher
    public void gotWeapon() {
        updateMissions(Mission.MissionType.GetWeapon);
    }

    //Called by BatteryManager
    public void gotBattery() {
        updateMissions(Mission.MissionType.GetBattery);
    }

    //Called by PlayerMelee
    public void gotMelee() {
        updateMissions(Mission.MissionType.MeleeEnemy);
    }

    //Called by Gun
    public void gotReload() {
        updateMissions(Mission.MissionType.PressReload);
    }

    public void gotFire() {
        updateMissions(Mission.MissionType.PressFire);
    }

    private void updateMissions(Mission.MissionType type) {
        foreach (Mission mission in currentMissions) {
            if (mission.missionType == type) {
                mission.value++;
                if (mission.value >= mission.goalValue) {
                    currentMissions.Remove(mission);
                }
                break;
            }
        }
    }
	
    //Get one weapon and one battery
	void StartMission1() {

        objectiveGUI.SetText("Get a weapon and a battery");

        //if (weaponSwitcher.totalWeapons <= 0)
            currentMissions.Add(new Mission(Mission.MissionType.GetWeapon,Mission.CheckComponent.WeaponSwitcher,0,1)); //Get Weapon Mission
        //if (batteryManager.batteries.Count <= 0)
            currentMissions.Add(new Mission(Mission.MissionType.GetBattery,Mission.CheckComponent.BatteryManager,0,1)); //Get Battery Mission
    
    missionNumber = 1;
    missionComplete.RemoveListener(StartMission1);
    missionComplete.AddListener(StartMission2);
    hasEvent = true;
    }

    

    //Mission 2
    void StartMission2() {
        objectiveGUI.SetText("The battery's dead! Go up to glowing enemies and hold F to melee kill");

        testEnemy.SetActive(true); //Test enemy

        currentMissions.Add(new Mission(Mission.MissionType.MeleeEnemy,Mission.CheckComponent.PlayerMelee,0,1)); //Melee Kill Enemy Mission

        missionNumber = 2;
        missionComplete.RemoveListener(StartMission2);
        missionComplete.AddListener(StartMission3);
        hasEvent = true;
    }

    //Mission 3
    void StartMission3() {
        objectiveGUI.SetText("Press R to reload and left-click to fire");

        currentMissions.Add(new Mission(Mission.MissionType.PressReload,Mission.CheckComponent.Gun,0,1)); //Reload Weapon Mission
        currentMissions.Add(new Mission(Mission.MissionType.PressFire,Mission.CheckComponent.Gun,0,1)); //Fire Gun Mission

        missionNumber = 3;
        missionComplete.RemoveListener(StartMission3);
        missionComplete.AddListener(StartMission4);
        hasEvent = true;
    }

    void StartMission4() {
        objectiveGUI.SetText("Go outside");

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