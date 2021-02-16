using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//TODO: transform.SetSiblingIndex() and transform.GetSiblingIndex()
public class BatteryManager : MonoBehaviour
{
    [Header("This script goes on an empty BatteryHolder object on the player, with the children being the batteries")]

    public Battery[] batteries;
    int maxSize = 1;
    [SerializeField] public BatteryIcons[] icons;
    public enum Type {AA, AAA};
    public enum State {Inventory, InUse, Charging, None};
    [SerializeField] public Color[] iconColors;

    void Start()
    {   
        refreshBatteryArray();
    }

    /*
        Must be called any time there is a change in the order of batteries, a new battery is added, or a battery is removed
    */
    public void refreshBatteryArray() {
        batteries = GetComponentsInChildren<Battery>(); //Also has a "includeInactive" boolean parameter

        int i=0; 
        foreach (Battery battery in batteries) { //Assigns each battery an icon in the UI
            icons[i].battery = battery;
            i++;
        }

        
        //Debug.Log("Number of Batteries: " + batteries.Length);
    }


    //Should it go to the right?
    //Gets the appropriate battery for the gun requesting it. So far just returns the first one!
    public Battery getBattery(int amountNeeded, Battery oldBattery = null) {

        int i=1; //Really only used for the toString()

        foreach (Battery battery in batteries)
        {
            if (battery.checkCharge(amountNeeded) && battery.state == State.Inventory) { //Will only use batteries that can fire at least once and are neither already in use or charging
                Debug.Log(battery.toString(i,"GETTING"));
                battery.changeState(State.InUse);

                oldBattery?.changeState(State.Inventory); //The question mark checks if the object is null before calling method

                return battery;
            }
            i++;
        }

        oldBattery?.changeState(State.Inventory); //Can't get new battery, but still taking out old battery

        return null;
    }

    /*
        Returns the battery being charged
    */
    public Battery chargeBattery(float amount) {

        int i=1; //just for debugging
        foreach (Battery battery in batteries) {
            if (battery.canCharge() && battery.state != State.InUse) {

                //Debug.Log(battery.state);

                if (battery.charge + amount > battery.maxCharge) {  //Allows overflow into other batteries. Doesn't return, so it'll look for the next battery
                    float amountLeft = battery.tilCharged();
                    battery.chargeIt(amountLeft);
                    amount -= amountLeft;
                    //battery.changeState(State.Charging);
                    //Debug.Log("Charged fully, " + amount + "P left");
                    //Debug.Log(battery.toString(i,"CHARGING P"));
                } else {                                            //Will just charge the one battery and return
                    battery.chargeIt(amount);
                    //battery.changeState(State.Charging);
                    //Debug.Log("Charged " + amount + "P ");
                    //break;
                    //Debug.Log(battery.toString(i,"CHARGING"));
                    return battery;
                }
            }
            i++;
        }
        return null;
    }
}
