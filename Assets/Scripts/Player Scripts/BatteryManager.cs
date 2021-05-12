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
    [SerializeField] public List<GameObject> icons;
    [SerializeField] public BatteryClicker batteryClicker;
    //[SerializeField] public BatteryIcons icon;
    [SerializeField] public Sprite aaaSprite;
    [SerializeField] public GameObject uiHolder;
    public enum Type {AAA, D, Car, NineVolt};
    public enum State {Inventory, InUse, Charging, None};
    public int chargeCost = 0;
    public GameObject blueFilter;
    public GameObject aaaIcon;
    public GameObject dIcon;
    public GameObject carIcon;
    public GameObject nineVIcon;
    public int spacer = 50; //distance between each icon
    float leftSpace = 50;
    [SerializeField] public MissionManager mm;
    [SerializeField] public WeaponSwitcher weaponSwitcher;
    public bool tellMM = false;
    public bool showBatteryName = false;
    public bool batteryEquipped = false;

    void Awake() {
        if (batteries == null)
            return;
        
        refreshBatteryArray();
    }

    void Start()
    { 
        Debug.Log("ON START");

        //blueFilter = GameObject.FindGameObjectWithTag("Filter");
        batteryClicker = GetComponent<BatteryClicker>();
        batteries = new ArrayList();
        icons = new List<GameObject>();
        refreshBatteryArray();
    }

    //Called by SaveData
    public void clearBatteryAray() {
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
        batteries.Clear();
        foreach (GameObject icon in icons) {
            Destroy(icon);
        }
        icons.Clear();
        leftSpace = 50f;
    }

    public void refreshBatteryArray() {

        Debug.Log("!!! REFRESH BATTERY ARRAY !!!");

        batteries.Clear(); //Clears array

        batteries.AddRange(GetComponentsInChildren<Battery>()); //Adds all batteries that are currently children of BatteryManager, also includes a "includeInactive" boolean parameter

        numOfBatteries = GetComponentsInChildren<Battery>().Length;

        Debug.Log(numOfBatteries);

        refreshIcons();

        
        //Debug.Log("Number of Batteries: " + batteries.Length);
    }

    public void refreshIcons() {

        //Debug.Log("!!! REFRESH ICONS !!!");

        foreach(GameObject icon in icons) {
            Destroy(icon);
        }
        icons.Clear();

        leftSpace = 50f;
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
            GameObject icon;

            switch (battery.getType()) {
                case BatteryManager.Type.AAA:
                    icon = aaaIcon;
                    break;
                case BatteryManager.Type.D:
                    icon = dIcon;
                    break;
                case BatteryManager.Type.Car:
                    icon = carIcon;
                    break;
                case BatteryManager.Type.NineVolt:
                    icon = nineVIcon;
                    break;
                default:
                    icon = aaaIcon;
                    break;
            }

            GameObject newIcon = Instantiate(icon,Vector3.zero,Quaternion.identity);
            RectTransform newRect = newIcon.GetComponent<RectTransform>();
            BatteryIcons bi = newIcon.GetComponentInChildren<BatteryIcons>();
            newRect.SetParent(uiHolder.transform);
            newRect.localScale = Vector3.one; //Scales icon to (1,1,1)
            bi.setPosition(leftSpace);
            bi.setWidth(newRect.rect.width);
            newRect.localPosition = new Vector3(bi.position,25,0); //Shifts icon
            leftSpace += spacer + newRect.rect.width;
            bi.setBattery(battery);
            bi.setClicker(batteryClicker);
            bi.setTextShow(showBatteryName);
            newIcon.SetActive(true); //Show it!
            icons.Add(newIcon);
            i++;
        }
    }

    //Gets the appropriate battery for the gun requesting it. Goes to the right
    public Battery getBattery(BatteryManager.Type type, int amountNeeded, Battery oldBattery = null) {

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

        batteryEquipped = false;

        foreach(Battery battery in rightBatteries) {
            if (battery.checkMatch(type,amountNeeded) && battery != oldBattery) { //Will only use batteries that can fire at least once and is not the same battery (can be changed to check state)
                //Debug.Log(battery.toString(i,"GETTING"));
                battery.changeState(State.InUse);
                AudioManager.instance?.Play("Bloop");
                batteryEquipped = true;

                oldBattery?.changeBackState(); //The question mark checks if the object is null before calling method

                return battery;
            }
        }

        foreach(Battery battery in leftBatteries) {
            if (battery.checkMatch(type,amountNeeded) && battery != oldBattery) { //Will only use batteries that can fire at least once and are is not the same battery (can be changed to check state)
                //Debug.Log(battery.toString(i,"GETTING"));
                battery.changeState(State.InUse);
                AudioManager.instance?.Play("Bloop");
                batteryEquipped = true;

                oldBattery?.changeBackState(); //The question mark checks if the object is null before calling method

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
            if (battery.canCharge()) { //I decided against excluding batteries in use. If you want to add that back in, add "&& battery.state != State.InUse"

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
        batteryEquipped = false;
    }

    public void chargeSpecificBattery(float amount, Battery inBattery) {
        if (inBattery.canCharge()) {
            if (inBattery.charge + amount > inBattery.maxCharge) {
                float amountLeft = inBattery.tilCharged();
                inBattery.chargeIt(amountLeft);
                amount -= amountLeft;
                chargeBattery(amount); //Charges the other batteries like normal
            } else {
                inBattery.chargeIt(amount);
            }
        }
    }
    
    //A cheat, called by MouseLook
    public void fillAllBatteries() {
        foreach (Battery battery in batteries) {
            battery.chargeIt(battery.maxCharge - battery.charge);
        }
    }

    public void swapBatteries(Battery battery1, Battery battery2) {
        int loc1, loc2;

        loc1 = batteries.IndexOf(battery1);
        loc2 = batteries.IndexOf(battery2);

        batteries[loc1] = battery2;
        batteries[loc2] = battery1;

        refreshIcons();
        
        /*
        GameObject tempIcon = icons[loc1];
        icons[loc1] = icons[loc2];
        icons[loc2] = tempIcon;
        icons[loc1].GetComponent<RectTransform>().localPosition = new Vector3(50+spacer*loc1,25,0); //Shifts icon1
        icons[loc2].GetComponent<RectTransform>().localPosition = new Vector3(50+spacer*loc2,25,0); //Shifts icon2
        */
    }

    public void addBattery(Battery newBattery) { //Lite version of refreshBatteryArray and refreshIcons
        batteries.Add(newBattery);
        numOfBatteries = GetComponentsInChildren<Battery>().Length;
        
        GameObject icon;

        switch (newBattery.getType()) {
            case BatteryManager.Type.AAA:
                icon = aaaIcon;
                break;
            case BatteryManager.Type.D:
                icon = dIcon;
                break;
            case BatteryManager.Type.Car:
                icon = carIcon;
                break;
            case BatteryManager.Type.NineVolt:
                icon = nineVIcon;
                break;
            default:
                icon = aaaIcon;
                break;
        }

        
        GameObject newIcon = Instantiate(icon,Vector3.zero,Quaternion.identity);
        RectTransform newRect = newIcon.GetComponent<RectTransform>();
        newRect.SetParent(uiHolder.transform);
        newRect.localScale = Vector3.one; //Scales icon to (1,1,1)
        newRect.localPosition = new Vector3(leftSpace+spacer,25,0); //Shifts icon
        leftSpace += spacer + newRect.rect.width;
        newIcon.GetComponentInChildren<BatteryIcons>().setBattery(newBattery);
        newIcon.GetComponentInChildren<BatteryIcons>().setClicker(batteryClicker);
        newIcon.GetComponentInChildren<BatteryIcons>().setTextShow(showBatteryName);
        newIcon.SetActive(true); //Show it!
        icons.Add(newIcon);
        

        //refreshIcons(); //Thought I could go without doing this everytime, but might have to

        if (tellMM && mm) {
            mm.gotBattery();
        }

        if (!batteryEquipped) { //If there isn't any battery equpped, and there is a new battery, try to load it right in
            weaponSwitcher.autoReload();
        }
    }

    public void removeBattery(Battery removeB) { //Lite version of refreshBatteryArray and refreshIcons
        //batteries.Remove(removeB);
        removeB.gameObject.transform.SetParent(null);
        Destroy(removeB.gameObject);
        refreshBatteryArray();

        if (tellMM && mm) {
            mm.removeBattery();
        }

    }

    public bool removeBattery(Type removeType) {
        foreach(Battery battery in batteries) {
            if (battery.checkMatch(removeType,battery.maxCharge)) {
                //batteries.Remove(battery);
                //Debug.Log("RB: Match: " + removeType);
                battery.gameObject.transform.SetParent(null);
                Destroy(battery.gameObject);
                refreshBatteryArray();

                if (tellMM && mm) {
                    mm.removeBattery();
                }
                return true;
            }
        }
        return false;
    }

    public void changeShowBatteryName(bool change) {
        //if (inShow == showBatteryName)
        //    return;
        Debug.Log("Options: Show Battery Name - " + change);
        
        showBatteryName = change;

        foreach(GameObject icon in icons) {
            icon.GetComponentInChildren<BatteryIcons>().setTextShow(showBatteryName);
        }
    }
}