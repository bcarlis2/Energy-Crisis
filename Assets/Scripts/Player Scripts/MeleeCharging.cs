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

public class MeleeCharging : MonoBehaviour {
    [SerializeField] BatteryManager bm;
    [SerializeField] Gun playerGun;
    //Outline outline;
    [SerializeField] PlayerMelee playerMelee;
    Battery battery;
    Battery oldBattery;
    Battery startBattery;
    float chargeAmount;
    float seconds = 2.5f;
    float buffer = 0;
    float intervals = 0.1f;
    public bool charging = true; //Player is charging
    float give = 0f;
    [SerializeField] public GameObject blueFilter;

    //Debugging
    public float totalCharge = 0;

    //Starting Values
    [SerializeField] float startingChargeAmount = 500f;
    [SerializeField] float startingInterval = 0.1f;
    [SerializeField] float startingSeconds = 2.5f;

    void Start()
    {
    }

    void OnEnable() {
        charging = true;
        chargeAmount = startingChargeAmount;
        intervals = startingInterval;
        seconds = startingSeconds;
        buffer = 0;
        give = 0f;

        AudioManager.instance.Play("MeleeCharging");

        startBattery = playerGun.battery;
        //outline = playerGun.gameObject.GetComponent<Outline>();
        //outline.enabled = true;
    }

    void Update()
    {
        buffer -= Time.deltaTime;
        blueFilter.SetActive(charging); //Might get overwritten by other charging fields

        if (!charging) {
            battery?.changeState(BatteryManager.State.Inventory); //Resets the state of the last battery to charge
            blueFilter.SetActive(false);
            Debug.Log("Stop Melee from MeleeCharging (No Longer Charging)");
            playerMelee.stopMelee();
            //outline.enabled = false;
            this.enabled = false;
            return;
        }

        if (buffer <= 0) {
            give = chargeAmount / (seconds / intervals);
            totalCharge += give;
            //Debug.Log(chargeAmount + "/" + seconds + "=" + give);
            chargeAmount -= give;

            //battery?.changeState(BatteryManager.State.Inventory); //Resets the state in case it's not charging again

            if (startBattery && startBattery.canCharge()) {
                bm.chargeSpecificBattery(give,startBattery);
                //startBattery.changeState(BatteryManager.State.Charging); //FLICKERS
            } else {

                if (battery) {
                    oldBattery = battery;
                } else {
                    oldBattery = null;
                }

                battery = bm.chargeBattery(give); //Tells the Battery Manager to find a battery and charge it, and returns the battery reference

                if (battery != oldBattery) {
                    oldBattery?.changeState(BatteryManager.State.Inventory);
                }
                battery?.changeState(BatteryManager.State.Charging);
            }

            seconds -= intervals;
            buffer = intervals;
        }

        if (seconds <= 0 || !charging) {
            battery?.changeState(BatteryManager.State.Inventory); //Resets the state of the last battery to charge
            blueFilter.SetActive(false);
            Debug.Log("Stop Melee from MeleeCharging");
            playerMelee.stopMelee();
            //outline.enabled = false;
            this.enabled = false;
        }
    }

    public void setGun(Gun inGun) {
        playerGun = inGun;
    }
}