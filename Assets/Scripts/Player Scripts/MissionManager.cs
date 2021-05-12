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
        public enum MissionType {GetWeapon,GetBattery,MeleeEnemy,PressReload,PressFire,KillEnemy,Interact,Trigger,RemoveBattery,WipeSpawner,Charge}
        public enum CheckComponent {WeaponSwitcher,BatteryManager,PlayerMelee,Gun,AllAttacks,Interactable,EnemySpawner,Battery}

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

    [SerializeField] public WeaponSwitcher weaponSwitcher;
    [SerializeField] public BatteryManager batteryManager;
    [SerializeField] public PlayerMelee playerMelee;
    [SerializeField] public Interactable[] interactables;
    [SerializeField] public Gun[] guns;
    [SerializeField] public EnemySpawner[] enemySpawners;
    [SerializeField] public TextMeshProUGUI objectiveGUI;
    [SerializeField] public Battery battery;
    [SerializeField] public List<Mission> currentMissions = new List<Mission>();
    public bool checkWeaponSwitcher = false;
    public bool checkBatteryManager = false;
    public bool checkPlayerMelee = false;
    public bool checkGun = false;
    public bool checkTarget = false;
    public int missionNumber = 0;
    public UnityEvent missionComplete = new UnityEvent();
    public bool hasEvent = false;
    //public bool killedAnEnemy = false;

    #endregion

    #region Unity Methods

    public void Start()
    {
        checkSecondSpawn();
        resetReferences();
        StartMission1();

    }

    public void Update()
    {

        //Debug.Log(killedAnEnemy);

        if (!hasEvent)
            return;

        //Mission Complete, move on to next mission
        if (currentMissions.Count == 0) {
            Debug.Log("Mission #" + missionNumber + "Complete!");
            hasEvent = false;

            removeComponents();

            missionComplete.Invoke();
        }

        /*
        //Updates which components should report back here
        bool keepWeaponSwitcher = false;
        bool keepBatteryManager = false;
        bool keepPlayerMelee = false;
        bool keepGun = false;
        bool keepTarget = false;

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
                case Mission.CheckComponent.Target:
                    keepTarget = true;
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

        if (checkTarget != keepTarget && target != null) {
            target.tellMM = keepTarget;
            checkTarget = keepTarget;
        }
        */
    }

    public void resetReferences() {
        /*
        [SerializeField] public WeaponSwitcher weaponSwitcher;
        [SerializeField] public BatteryManager batteryManager;
        [SerializeField] public PlayerMelee playerMelee;
        [SerializeField] public TextMeshProUGUI objectiveGUI;
        */

        weaponSwitcher = transform.parent.GetComponentInChildren<WeaponSwitcher>();
        batteryManager = transform.parent.GetComponentInChildren<BatteryManager>();
        playerMelee = transform.parent.GetComponent<PlayerMelee>();
        objectiveGUI = transform.parent.GetComponentInChildren<ObjectiveText>().gameObject.GetComponent<TextMeshProUGUI>();

        weaponSwitcher.mm = this;
        batteryManager.mm = this;
        playerMelee.mm = this;

    }

    public void getComponents() {
        foreach (Mission mission in currentMissions) {
            switch (mission.toCheck) {
                case Mission.CheckComponent.WeaponSwitcher:
                    weaponSwitcher.tellMM = true;
                    break;
                case Mission.CheckComponent.BatteryManager:
                    batteryManager.tellMM = true;
                    break;
                case Mission.CheckComponent.PlayerMelee:
                    playerMelee.tellMM = true;
                    break;
                case Mission.CheckComponent.Gun:
                    guns = weaponSwitcher.gameObject.GetComponentsInChildren<Gun>(true);
                    foreach (Gun gun in guns) {
                        gun.tellMM = true;
                    }
                    break;
                case Mission.CheckComponent.AllAttacks:
                    playerMelee.tellMM = true;
                    guns = weaponSwitcher.gameObject.GetComponentsInChildren<Gun>(true); //Includes inactive!
                    foreach (Gun gun in guns) {
                        gun.tellMM = true;
                    }
                    break;
                case Mission.CheckComponent.Interactable:
                    foreach (Interactable interactable in interactables) {
                        interactable.tellMM = true;
                    }
                    break;
                case Mission.CheckComponent.EnemySpawner:
                    foreach (EnemySpawner enemySpawner in enemySpawners) {
                        if (enemySpawner.mm == null)
                            enemySpawner.mm = this;
                        enemySpawner.tellMM = true;
                    }
                    break;
                case Mission.CheckComponent.Battery:
                    Debug.Log("CheckComponent Battery");
                    battery.tellMM = true;
                    Debug.Log("tellMM : " + battery.tellMM);
                    break;
            }
        }
    }

    public void removeComponents() {
        weaponSwitcher.tellMM = false;
        batteryManager.tellMM = false;
        playerMelee.tellMM = false;

        if (guns != null) {
            foreach (Gun gun in guns) {
                gun.tellMM = false;
            }
        }

        if (interactables != null) {
            foreach (Interactable interactable in interactables) {
                interactable.tellMM = false;
            }
        }

        if (enemySpawners != null) {
            foreach (EnemySpawner enemySpawner in enemySpawners) {
                enemySpawner.tellMM = false;
            }
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

    //Called by BatteryManager
    public void removeBattery() {
        updateMissions(Mission.MissionType.RemoveBattery);
    }

    //Called by Gun
    public void gotReload() {
        updateMissions(Mission.MissionType.PressReload);
    }


    //Called by Gun
    public void gotFire() {
        updateMissions(Mission.MissionType.PressFire);
    }

    //Called by All Attacks (Gun and PlayerMelee)
    public void killedEnemy() {
        Debug.Log("MM: KILLED ENEMY");
        updateMissions(Mission.MissionType.KillEnemy);
    }

    //Called by Interactable
    public void gotInteract() {
        updateMissions(Mission.MissionType.Interact);
    }

    //Called by Interactable
    public void gotTrigger() {
        updateMissions(Mission.MissionType.Trigger);
    }

    //Called by EnemySpawner
    public void wipedSpawner() {
        updateMissions(Mission.MissionType.WipeSpawner);
    }

    //Called by Battery
    public void gotCharged() {
        updateMissions(Mission.MissionType.Charge);
    }

    /*
    //Called by Target
    public void killedEnemy() { //Doesn't work for some god-forsaken reason
        Debug.Log("MM: Killed Enemy");
        killedAnEnemy = true;
        updateMissions(Mission.MissionType.KillEnemy);
    }
    */

    public void updateMissions(Mission.MissionType type) {
        Debug.Log("Type: " + type + ", Count: " + currentMissions.Count);
        foreach (Mission mission in currentMissions) {
            Debug.Log(mission.missionType + " " + type);
            Debug.Log(mission.missionType == type);
            if (mission.missionType == type) {
                mission.value++;
                Debug.Log("MM: Missison Value: " + mission.value);
                if (mission.value >= mission.goalValue) {
                    Debug.Log("MM: Removed Mission");
                    currentMissions.Remove(mission);
                    Debug.Log("After removal, mission count = " + currentMissions.Count);
                }
                break;
            }
        }
    }

    public virtual void StartMission1() { }

    public virtual void checkSecondSpawn() { }

	/*
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
        objectiveGUI.SetText("Go outside through front door when you are ready");

        door.GetComponent<Outline>().enabled = true;
        door.GetComponent<Door>().enabled = true;

        missionNumber = 4;
        missionComplete.RemoveListener(StartMission4);

        //This will end with loading to a new scene


    }
    */

    void NoMission() {
        objectiveGUI.SetText("No Mission");
    }


    #endregion
}