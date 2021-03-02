using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//transform.SetSiblingIndex() and transform.GetSiblingIndex() ?
public class BatteryManager : MonoBehaviour
{
    [Header("This script goes on an empty BatteryHolder object on the player, with the children being the batteries")]

    public ArrayList batteries;
    int maxSize;
    [SerializeField] public int numOfBatteries;
    [SerializeField] public ArrayList icons;
    //[SerializeField] public BatteryIcons icon;
    [SerializeField] public Sprite aaaSprite;
    [SerializeField] public GameObject uiHolder;
    public enum Type {AA, AAA};
    public enum State {Inventory, InUse, Charging, None};
    public int chargeCost = 0;
    public GameObject blueFilter;
    public GameObject aaaIcon;

    void Start()
    { 
        blueFilter = GameObject.FindGameObjectWithTag("Filter");  
        batteries = new ArrayList();
        icons = new ArrayList();
        refreshBatteryArray();
    }

    /*
        Must be called any time there is a change in the order of batteries, a new battery is added, or a battery is removed
    */
    public void refreshBatteryArray() {

        batteries.Clear(); //Clears array

        batteries.AddRange(GetComponentsInChildren<Battery>()); //Adds all batteries that are currently children of BatteryManager, also includes a "includeInactive" boolean parameter

        numOfBatteries = GetComponentsInChildren<Battery>().Length;

        refreshIcons();

        
        //Debug.Log("Number of Batteries: " + batteries.Length);
    }

    public void refreshIcons() { //TODO: Maybe make this into a prefab instead...

        foreach(GameObject icon in icons) {
            Destroy(icon);
        }
        icons.Clear();

        int spacer=35; //distance between each icon
        int i=0;
        foreach (Battery battery in batteries) {
            /*
            GameObject newIcon = new GameObject("BatteryIcon"); //Makes new GameObject
            Image newImage = newIcon.AddComponent<Image>(); //Makes new Image component
            newImage.sprite = aaaSprite; //Assigns the sprite according to battery type (currently just gives it AAA)
            newIcon.GetComponent<RectTransform>().SetParent(uiHolder.transform); //Aligns it with the holder on the Canvas
            newIcon.GetComponent<RectTransform>().localScale = new Vector3(0.25f,1,1); //Scales icon to look more like a battery
            newIcon.GetComponent<RectTransform>().localPosition += new Vector3(100+spacer*i,50,0); //Shifts icon
            newIcon.AddComponent<Slider>();
            newIcon.AddComponent<ProgressBar>();
            newIcon.AddComponent<BatteryIcons>();
            newIcon.GetComponent<BatteryIcons>().setBattery(battery);
            newIcon.SetActive(true); //Show it!
            icons.Add(newIcon);
            i++;
            */
            GameObject newIcon = Instantiate(aaaIcon,Vector3.zero,Quaternion.identity);
            newIcon.GetComponent<RectTransform>().SetParent(uiHolder.transform);
            newIcon.GetComponent<RectTransform>().localScale = Vector3.one; //Scales icon to (1,1,1)
            newIcon.GetComponent<RectTransform>().localPosition += new Vector3(50+spacer*i,25,0); //Shifts icon
            newIcon.GetComponentInChildren<BatteryIcons>().setBattery(battery);
            newIcon.SetActive(true); //Show it!
            icons.Add(newIcon);
            i++;
        }
    }

    //Gets the appropriate battery for the gun requesting it. Goes to the right
    public Battery getBattery(int amountNeeded, Battery oldBattery = null) {

        int oldIndex;
        int leftLength;
        int rightLength;

        if (oldBattery && batteries.Contains(oldBattery)) {
            oldIndex = batteries.IndexOf(oldBattery);
            rightLength = numOfBatteries - oldIndex - 1;
            leftLength = oldIndex;
        } else {
            oldIndex = -1;
            rightLength = numOfBatteries;
            leftLength = 0;
        }

        //Debug.Log("OLDINDEX " + oldIndex + " RL: " + rightLength + " LL: " + leftLength);

        ArrayList rightBatteries = batteries.GetRange(oldIndex+1,rightLength);
        //Debug.Log("RL: " + rightLength);
        ArrayList leftBatteries = batteries.GetRange(0,leftLength);
        //Debug.Log(" LL: " + leftLength);

        foreach(Battery battery in rightBatteries) {
            if (battery.checkCharge(amountNeeded) && battery.state == State.Inventory) { //Will only use batteries that can fire at least once and are neither already in use or charging
                //Debug.Log(battery.toString(i,"GETTING"));
                battery.changeState(State.InUse);

                oldBattery?.changeState(State.Inventory); //The question mark checks if the object is null before calling method

                return battery;
            }
        }

        foreach(Battery battery in leftBatteries) {
            if (battery.checkCharge(amountNeeded) && battery.state == State.Inventory) { //Will only use batteries that can fire at least once and are neither already in use or charging
                //Debug.Log(battery.toString(i,"GETTING"));
                battery.changeState(State.InUse);

                oldBattery?.changeState(State.Inventory); //The question mark checks if the object is null before calling method

                return battery;
            }
        }

        oldBattery?.changeState(State.Inventory); //Can't get new battery, but still taking out old battery

        return null;


        /* OLD METHOD, JUST GOES FROM INDEX 0 EVERY TIME
        foreach (Battery battery in batteries)
        {
            if (battery.checkCharge(amountNeeded) && battery.state == State.Inventory) { //Will only use batteries that can fire at least once and are neither already in use or charging
                //Debug.Log(battery.toString(i,"GETTING"));
                battery.changeState(State.InUse);
                oldBattery?.changeState(State.Inventory); //The question mark checks if the object is null before calling method
                return battery;
            }
        }
        oldBattery?.changeState(State.Inventory); //Can't get new battery, but still taking out old battery
        return null;
        */
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

    public void unload() { //Unloads all batteries from any guns
        foreach (Battery battery in batteries) {
            battery.changeState(State.Inventory);
        }
    }
}