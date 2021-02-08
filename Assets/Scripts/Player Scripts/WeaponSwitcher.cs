using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    [Header("This script goes on an empty WeaponHolder object on the player, with the children being the weapons")]

    public int selectedWeapon = 0;

    void Start()
    {    

    }

    void Update()
    {
        int previousSelectedWepaon = selectedWeapon;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f) { //Scrolled up
            if (selectedWeapon >= transform.childCount - 1) {
                selectedWeapon = 0; //Loops around to beginning of the weapon list
            } else {
                selectedWeapon++; //Weapon to the right
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f) { //Scrolled down
            if (selectedWeapon <= 0) {
                selectedWeapon = transform.childCount - 1; //Look around to end of the weapon list
            } else {
                selectedWeapon--; //Weapon to the left
            }
        }

        //Pressing the 1 or 2 key gets the first or second weapon, may expand later
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            selectedWeapon = 0;
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >= 2) {
            selectedWeapon = 1;
        }

        //Selects the weapon at the end of the update rather an immediately to prevent multiple inputs causing confusion
        if (previousSelectedWepaon != selectedWeapon) {
            SelectWeapon();
        }
    }

    void SelectWeapon() {
        int i=0;

        //Goes through the weapon array, activating the chosen weapon and deactivating everything else
        foreach (Transform weapon in transform) {
            if (i == selectedWeapon) {
                weapon.gameObject.SetActive(true);
            } else {
                weapon.gameObject.SetActive(false);
            }
            i++;
        }
    }
}
