using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalBattery : MonoBehaviour
{
    Battery battery;
    BatteryManager bm;
    int maxBatteries = 20;

    // Start is called before the first frame update
    void Start()
    {
        battery = GetComponent<Battery>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) {

            bm = other.gameObject.GetComponentInChildren<BatteryManager>();
            if (bm.numOfBatteries < maxBatteries) {
                transform.SetParent(bm.gameObject.transform); //Gets the Player's GameObject's child's BatteryManager's parent's transform
                bm.refreshBatteryArray();
                GetComponent<Collider>().enabled = false;
                GetComponent<Renderer>().enabled = false;
            }

        }
    }
}
