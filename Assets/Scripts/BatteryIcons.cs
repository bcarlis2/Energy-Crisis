using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryIcons : MonoBehaviour
{
    [SerializeField] public Battery battery;
    Image image;
    ProgressBar chargeBar;
    [SerializeField] Color[] iconColors;
    RectTransform rt;

    public BatteryManager.State status;
    public float charge;
    bool charging = false;

    void Start()
    {
        image = GetComponent<Image>();
        chargeBar = GetComponent<ProgressBar>();
        rt = GetComponent<RectTransform>();
        chargeBar.setMaxValue(battery.maxCharge,battery.charge);
        //Debug.Log("SETTED MAX " + battery.maxCharge);

        status = BatteryManager.State.None;
        charge = 0;
    }

    public void setBattery(Battery inB) {
        battery = inB;
    }

    //Updates the UI elements for the batteries
    void FixedUpdate()
    {
        //Updates BatteryIcon state color
        BatteryManager.State currentState = battery.state;
        if (currentState != status) {
            status = currentState;
            switch (status) {
                case BatteryManager.State.Inventory:
                    charging = false;
                    //status = "Inventory";
                    //image.color = Color.white;
                    rt.localScale = new Vector3(0.25f,1,1);
                    break;
                case BatteryManager.State.InUse:
                    charging = false;
                    //status = "Using";
                    //image.color = Color.yellow;
                    rt.localScale = new Vector3(0.25f,1.5f,0);
                    break;
                case BatteryManager.State.Charging:
                    charging = true;
                    //status = "Charging";
                    //image.color = Color.cyan;
                    //rt.localScale = new Vector3(0.25f,1.25f,0);
                    break;
            }
        }

        //Updates BatteryIcon charge value. Just switching every time so that the colors are accurate
        float currentCharge = battery.charge;
        charge = currentCharge;
        chargeBar.SetValue(charge,charging);
            
    }
}