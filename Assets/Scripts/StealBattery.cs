/*
	Project:    Energy Crisis
	
	Script:     StealBattery
	Desc:       Removes a specific type of battery from the player on interaction, used for power poles and generators
	
	Credits:	Brandon Carlisle
	
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealBattery : Interactable {

	#region Variables
	
    [SerializeField] public BatteryManager.Type batteryType;
    [SerializeField] public PowerPole powerPole;
	

    #endregion

    #region Methods

    public override void interact() {
        //base.interact();

        if (base.bm.removeBattery(batteryType)) {

            if (base.tellMM && base.mm) {
                base.mm.gotInteract();
            }

            if (powerPole) {
                powerPole.enabled = true;
                powerPole.startShocking();
            }
        }
    }
	
	

    #endregion
}