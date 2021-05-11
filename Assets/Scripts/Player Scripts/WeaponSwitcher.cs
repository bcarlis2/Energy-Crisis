using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    [Header("This script goes on an empty WeaponHolder object on the player, with the children being the weapons")]

    [SerializeField] BatteryManager bm;
    [SerializeField] public MissionManager mm;
    public int selectedWeapon = -1;
    public int totalWeapons = 0;
    private bool switchCooldown = false;
    public bool tellMM = false;

    void Start()
    {    

    }

    void Update()
    {
        totalWeapons = transform.childCount; //Probably don't need to check this every frame

        if (transform.childCount < 1)
            return;

        if (selectedWeapon < 0)
            selectedWeapon = 0;

        int previousSelectedWepaon = selectedWeapon;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f && !switchCooldown) { //Scrolled up
            if (selectedWeapon >= transform.childCount - 1) {
                selectedWeapon = 0; //Loops around to beginning of the weapon list
            } else {
                selectedWeapon++; //Weapon to the right
            }
            switchCooldown = true;
            Invoke(nameof(refreshSwitch),0.25f);
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f && !switchCooldown) { //Scrolled down
            if (selectedWeapon <= 0) {
                selectedWeapon = transform.childCount - 1; //Look around to end of the weapon list
            } else {
                selectedWeapon--; //Weapon to the left
            }
            switchCooldown = true;
            Invoke(nameof(refreshSwitch),0.25f);
        }

        if (Input.GetButtonDown("FillAll")) {
            bm.fillAllBatteries();
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

    void SelectWeapon(int specific = -1) {

        if (specific >= 0)
            selectedWeapon = specific;
        
        int i=0;

        bm.unload(); //Unloads all batteries to avoid confusion

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

    public GameObject getWeapon() {
        if (selectedWeapon < 0)
            return null;
            
        return transform.GetChild(selectedWeapon).gameObject;
    }

    public void equipNew() {
        SelectWeapon(transform.childCount -1);

        if (tellMM && mm) {
            mm.gotWeapon();
        }
    }

    public void refreshSwitch() {
        switchCooldown = false;
    }

    public void clearWeapons() {
        foreach (Transform weapon in transform) {
            Destroy(weapon.gameObject);
        }
    }
}
