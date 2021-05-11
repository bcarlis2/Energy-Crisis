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

public class PhysicalGun : MonoBehaviour {

	#region Variables
	
    WeaponSwitcher weaponSwitcher;
	SphereCollider trigger;
    Gun gunScript;

    #endregion

    #region Unity Methods

    public void Start()
    {
        trigger = GetComponent<SphereCollider>();
        gunScript = GetComponent<Gun>();

        //Adds outline at runtime
        //var outline = gameObject.AddComponent<Outline>();
        //outline.OutlineMode = Outline.Mode.OutlineVisible;
        //outline.OutlineColor = Color.blue;
        //outline.OutlineWidth = 10f;
    }

    public void Update()
    {
        //You're doing great!
    }

    #endregion

    #region Methods
	
	private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) {
            //Debug.Log("BATTERY COLLIDED");
            
            if (gunScript.bm == null) {
                gunScript.bm = other.gameObject.GetComponentInChildren<BatteryManager>();
            }

            if (gunScript.mouseLook == null) {
                gunScript.mouseLook = other.gameObject.GetComponentInChildren<MouseLook>();
            }

            if (gunScript.fpsCam == null) {
                gunScript.fpsCam = other.gameObject.GetComponentInChildren<Camera>();
            }

            weaponSwitcher = other.gameObject.GetComponentInChildren<WeaponSwitcher>();
            transform.SetParent(weaponSwitcher.gameObject.transform); //Gets the Player's GameObject's child's WeaponHolder's parent's transform

            if (gunScript.gunType == Gun.GunType.Pistol) {
                transform.localPosition = new Vector3(0.46f,-0.38f,0.9f); //Aligment I got from Unity Editor
                transform.localRotation = Quaternion.Euler(4.4f,169.87f,0f);
            } else if (gunScript.gunType == Gun.GunType.Shotgun) {
                transform.localPosition = new Vector3(0.61f, -0.23f, 0.95f);
                transform.localRotation = Quaternion.Euler(8.27f,-184.95f,0f);
            }

            //Turn off everything that makes it physical
            trigger.enabled = false;
            //Make it a functioning gun
            gunScript.enabled = true;
            //Equip gun
            weaponSwitcher.equipNew();
        }
    }

    #endregion
}