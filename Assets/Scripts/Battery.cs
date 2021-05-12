using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour
{
    [SerializeField] public float charge;
    public int maxCharge = 100;
    [SerializeField] public BatteryManager.Type type;
    [SerializeField] public BatteryManager.State state;
    public BatteryManager.State prevState;
    public MissionManager mm;
    public bool tellMM = false;
    public int place = -1;
    //BatteryManager bm;

    //For changing color
    //private Renderer renderer;
    //private MaterialPropertyBlock propBlock;

    void Start()
    {
        //bm = GameObject.FindWithTag("BatteryManager").GetComponent<BatteryManager>(); //Might take too long
        //type = BatteryManager.Type.AAA;
        state = BatteryManager.State.Inventory;

        //renderer = GetComponent<Renderer>();
    }


    void Update()
    {
        
    }

    public void use(int amount) {
        charge -= amount;

        if (charge < 0) {
            Debug.Log("A battery is in the negative??");
        }
    }

    public void chargeIt(float amount) {
        charge += amount;

        if (charge > maxCharge) {
            Debug.Log("A battery is overcharged??");
        }

        if (charge >= maxCharge)
            Debug.Log("Charged battery");

        if (charge >= maxCharge && mm && tellMM) {
            Debug.Log("MM: Charged battery");
            mm.gotCharged();
        }
    }

    public bool checkCharge(int amount) {
        return (charge >= amount);
    }

    public bool checkType(BatteryManager.Type inType) {
        return (type == inType);
    }

    public BatteryManager.Type getType() {
        return type;
    }

    public bool checkMatch(BatteryManager.Type inType, int amount) { //Checks type and charge in one function
        return (charge >= amount && type == inType);
    }

    public bool canCharge() {
        //Debug.Log("Can Charge...." + (charge < maxCharge));
        return ((charge < maxCharge));
    }

    public float tilCharged() {
        return maxCharge - charge;
    }

    //Just for debugging so far
    public string toString(int place = -1, string header = "") {
        this.place = place;
        return (header + " Battery #" + place + ": " + state);
        //return ("Battery #" + place + ": " + charge + "/" + maxCharge);
    }

    public string toString() {
        return ("Battery #" + place + ": " + state);
    }

    public void changeState(BatteryManager.State inState) {
        Debug.Log("Battery: Change State");
        
        if (state != BatteryManager.State.Charging)
            prevState = state;
        
        state = inState;
    }

    public void changeBackState() {
        Debug.Log("Battery: Change Back State");
        state = prevState;
    }
}