using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryIcons : MonoBehaviour
{
    public string status; //Just for debugging
    [SerializeField] public Battery battery;
    Image image;
    ProgressBar chargeBar;
    [SerializeField] Color[] iconColors;
    RectTransform rt;

    void Start()
    {
        image = GetComponent<Image>();
        chargeBar = GetComponent<ProgressBar>();
        rt = GetComponent<RectTransform>();
        chargeBar.setMaxValue(battery.maxCharge,battery.charge);
        Debug.Log("SETTED MAX " + battery.maxCharge);
    }

    public void setBattery(Battery inB) {
        battery = inB;
    }

    //Updates the UI elements for the batteries
    void FixedUpdate()
    {
        chargeBar.SetValue(battery.charge);

        //Not the best idea
        switch (battery.state) {
            case BatteryManager.State.Inventory:
                status = "Inventory";
                //image.color = Color.red;
                rt.localScale = new Vector3(0.25f,1,1);
                break;
            case BatteryManager.State.InUse:
                status = "Using";
                //image.color = Color.yellow;
                rt.localScale = new Vector3(0.25f,1.5f,0);
                break;
            case BatteryManager.State.Charging:
                status = "Charging";
                image.color = Color.cyan;
                //rt.localScale = new Vector3(0.25f,1.25f,0);
                break;
            }
    }
}
