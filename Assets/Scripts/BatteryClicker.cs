/*
	Project:	
	
	Script:		
	Desc:		
	
	Last Edit:	
	Credits:	Brandon Carlisle
	
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryClicker : MonoBehaviour {

	#region Variables
	
    BatteryManager bm;
	public Battery battery1, battery2;

    #endregion

    #region Unity Methods

    public void Start()
    {
        bm = GetComponent<BatteryManager>();
    }

    void OnDisable() {
        clearClicked();
    }

    public void Update()
    {
        //You're doing great!
    }

    #endregion

    #region Methods
	
	public void newClicked(Battery inBattery) {

        if (!battery1) {                    //First slot empty? Put it there
            battery1 = inBattery;

        } else if (battery1 == inBattery) { //Same battery that was already clicked on? Undo it
            battery1 = null;
        } else if (!battery2) {
            battery2 = inBattery;
            bm.swapBatteries(battery1,battery2); //Swap the batteries
            clearClicked();
        }
    }

    public void clearClicked() {
        battery1 = null;
        battery2 = null;
    }

    #endregion
}