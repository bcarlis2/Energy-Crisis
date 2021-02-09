using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingField : MonoBehaviour
{
    BatteryManager bm;
    Battery battery;
    [SerializeField] float chargeAmount = 100f;
    [SerializeField] float seconds = 5;
    float buffer = 0;
    [SerializeField] float intervals = 0.5f;
    bool charging;
    float give = 0f;

    void Start()
    {
    }

    void Update()
    {
        buffer -= Time.deltaTime;

        if (buffer <= 0) {
            give = chargeAmount / (seconds / intervals);
            //Debug.Log(chargeAmount + "/" + seconds + "=" + give);
            chargeAmount -= give;

            battery?.changeState(BatteryManager.State.Inventory); //Resets the state in case it's not charging again

            if (charging) {
                battery = bm.chargeBattery(give); //Tells the Battery Manager to find a battery and charge it, and returns the battery reference
                battery?.changeState(BatteryManager.State.Charging);
            }

            seconds -= intervals;
            buffer = intervals;
        }

        if (seconds <= 0) {
            battery?.changeState(BatteryManager.State.Inventory); //Resets the state of the last battery to charge
            Destroy(transform.parent.gameObject); //Destroys the enemy parent
            Destroy(this.gameObject); //Destroys self
        }

        /*
        if (timeRemaining > 0) {
            give = chargeAmount / timeRemaining;
            chargeAmount -= give;
            if (charging) {
                bm.chargeBattery(give);
                Debug.Log("CF: charging battery");
            }
        }
            */
        

        /*
            TIME REMAINING = 10:
                give = 100 / 10 = 10
                chargeAmount = 90
            TIME REAMINING = 9:
                give = 90 / 9 = 10
                chargeAmount = 80
        */
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) {

            if (bm == null) {
                bm = other.gameObject.GetComponentInChildren<BatteryManager>();
            }

            charging = true;

            //bm.chargeBattery();
            //Debug.Log("Charging Started");
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player")) {

            charging = false;
            //Debug.Log("Charging Stopped");
        }
    }
}
