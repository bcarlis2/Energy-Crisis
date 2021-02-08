using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: transform.SetSiblingIndex() and transform.GetSiblingIndex()
public class BatteryManager : MonoBehaviour
{
    [Header("This script goes on an empty BatteryHolder object on the player, with the children being the batteries")]

    public Battery[] batteries;
    int maxSize = 1;
    [SerializeField] public ProgressBar[] chargeBars;

    void Start()
    {
        batteries = new Battery[maxSize];
        
        for (int i=0; i<maxSize; i++) {
            batteries[i] = transform.GetChild(i).gameObject.GetComponent<Battery>();
            i++;
        }
    }


    //TODO: Add more charge bars
    //Updates the UI elements for the batteries
    void FixedUpdate()
    {
        chargeBars[0].SetValue(batteries[0].charge);
    }

    //TODO: Smarter battery selection
    //Gets the appropriate battery for the gun requesting it. So far just returns the first one!
    public Battery getBattery() {
        return batteries[0];
    }
}
