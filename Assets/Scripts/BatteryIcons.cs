using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryIcons : MonoBehaviour
{
    [SerializeField] public Battery battery;
    Image image;
    ProgressBar chargeBar;
    //[SerializeField] Color[] iconColors;
    RectTransform rt;

    public BatteryManager.State status;
    public float charge;
    bool charging = false;

    void Start()
    {
        image = GetComponent<Image>();
        chargeBar = GetComponent<ProgressBar>();
        rt = gameObject.transform.parent.GetComponent<RectTransform>();
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
        if (battery == null)
            return;

        //Updates BatteryIcon state size
        BatteryManager.State currentState = battery.state;

        if (currentState != status) {
            status = currentState;
            switch (status) {
                case BatteryManager.State.Inventory:
                    charging = false;
                    //status = "Inventory";
                    //image.color = Color.white;
                    rt.localScale = Vector3.one;
                    break;
                case BatteryManager.State.InUse:
                    charging = false;
                    //status = "Using";
                    //image.color = Color.yellow;
                    rt.localScale = new Vector3(1,1.5f,1);
                    break;
                case BatteryManager.State.Charging:
                    charging = true;
                    //status = "Charging";
                    //image.color = Color.cyan;
                    rt.localScale = Vector3.one;
                    break;
            }
        }

        //Updates BatteryIcon charge value. Just switching every time so that the colors are accurate
        float currentCharge = battery.charge;
        charge = currentCharge;
        chargeBar.SetValue(charge,charging);
            
    }
}