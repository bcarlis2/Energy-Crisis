﻿/*
	Project:    Energy Crisis
	
	Script:     MedKit
	Desc:       Gives health when interacted with
	
	Credits:	Brandon Carlisle
	
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedKit : Interactable {

	#region Variables
	
	//public PlayerHealth playerHealth;
    public int heal = 25;

    #endregion

    #region Methods
	
	public override void interact() {
        
        if (base.playerHealth.isFull())
            return;

        base.playerHealth.giveHealth(heal);
        AudioManager.instance.Play("Eat");
        Destroy(this.gameObject);
    }

    #endregion
}