using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalBattery : MonoBehaviour
{
    Battery battery;

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

            transform.SetParent(other.gameObject.GetComponentInChildren<BatteryManager>().gameObject.transform); //Gets the Player's GameObject's child's BatteryManager's parent's transform
            other.gameObject.GetComponentInChildren<BatteryManager>().refreshBatteryArray();
            GetComponent<Collider>().enabled = false;
            GetComponent<Renderer>().enabled = false;

        }
    }
}
