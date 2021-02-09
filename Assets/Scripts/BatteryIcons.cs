using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryIcons : MonoBehaviour
{
    public Battery battery;
    Image image;
    ProgressBar chargeBar;
    [SerializeField] Color[] iconColors;

    void Start()
    {
        image = GetComponent<Image>();
        chargeBar = GetComponentInChildren<ProgressBar>();
    }

    //Updates the UI elements for the batteries
    void FixedUpdate()
    {
        chargeBar.SetValue(battery.charge);

        //Not the best idea
        switch (battery.state) {
            case BatteryManager.State.Inventory:
                image.color = Color.red;
                break;
            case BatteryManager.State.InUse:
                image.color = Color.yellow;
                break;
            case BatteryManager.State.Charging:
                image.color = Color.green;
                break;
            }
    }
}
