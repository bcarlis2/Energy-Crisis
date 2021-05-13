/*
	Project:    Energy Crisis
	
	Script:     PlayerMelee
	Desc:       Handles player melee, lock-on, animation, buffer, and killing blows
	
	Credits:	Brandon Carlisle
	
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMelee : MonoBehaviour {

	#region Variables
	
    [SerializeField] GameObject meleeUI;

    public Transform enemy;
    public GameObject gun;
    public EnemyMovement enemyMovement;
	public Target target;

    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] MouseLook mouseLook;
    [SerializeField] public MeleeCharging meleeCharging;
    private WeaponSwitcher weaponSwitcher;
    private BatteryManager bm;
    private Hitmarker hitmarker;

    public bool isStabbing = false;
    bool canStab = true;
    bool simpleMeleeing = false;
    bool cantShowUI = false;
    public int stabDamage = 10;
    public int stabWait = 1;
    private float nextTimeToStab = 0f; //Has to start at zero
    public bool killingBlow = false;

    Vector3 originalGunLoc;

    [SerializeField] public MissionManager mm;
    public bool tellMM;

    #endregion

    #region Unity Methods

    public void Start()
    {
        weaponSwitcher = GetComponentInChildren<WeaponSwitcher>();
        hitmarker = GetComponentInChildren<Hitmarker>();
        bm = GetComponentInChildren<BatteryManager>();
    }

    public void Awake() {

    }

    public void Update()
    {
        if (!enemyMovement.playerInMeleeRange) {
            disableMelee();
            return;
        }

        if (!canStab && Time.time >= nextTimeToStab) {
            canStab = true;
        }

        if (meleeUI.activeSelf && !canStab) {
            meleeUI.SetActive(false);
            cantShowUI = true;
        }

        if (cantShowUI && canStab) {
            meleeUI.SetActive(true);
            cantShowUI = false;
        }

        if (Input.GetButtonDown("Melee") && canStab && !isStabbing) {
            startMelee();
        }

        if (Input.GetButton("Melee") && isStabbing) {
            if (killingBlow || simpleMeleeing) {
                keepMelee();
            } else {
                stopMelee();
            }
        }

        if (Input.GetButtonUp("Melee") && isStabbing && !simpleMeleeing) {
                stopMelee();
        }


    }

    #endregion

    #region Methods
	
	public void setTarget(EnemyMovement inMovement, Target inTarget, Transform inTrans) {
        enemyMovement = inMovement;
        target = inTarget;
        enemy = inTrans;
        meleeUI.SetActive(true);
        Debug.Log("Melee Possible");
    }

    void disableMelee() {
        Debug.Log("Melee No Longer Possible");
        meleeUI.SetActive(false);
        enemyMovement = null;
        target = null;
        enemy = null;
        this.enabled = false;
    }

    void startMelee() {
        Debug.Log("Start Melee");
        isStabbing = true;
        canStab = false;
        
        playerMovement.canMove = false;
        enemyMovement.canMove = false;
        mouseLook.holdMouse();
        mouseLook.playerLookAt(new Vector3(enemy.position.x, transform.position.y, enemy.position.z));

        gun = weaponSwitcher.getWeapon();

        if (gun) {
            originalGunLoc = gun.transform.localPosition;
            gun.transform.position = transform.position + transform.forward * 2f; //Makes gun lunge forward
        }

        if (target.health > stabDamage || !gun) { //Just take damage if they're not going to die
            dealDamage(target,stabDamage);
            simpleMelee();
        } else {
            Debug.Log("Starting Melee Charging...");
            killingBlow = true;
            dealDamage(target,stabDamage);
            meleeCharging.setGun(gun.GetComponent<Gun>());
            meleeCharging.enabled = true;
        }
    }

    void keepMelee() {
    }

    void simpleMelee() {
        simpleMeleeing = true;
        Invoke(nameof(stopMelee),0.5f);
    }

    public void stopMelee() { //Can stop from the EnemyMovement script if out of range
        if (!isStabbing) {
            Debug.Log("Conditional Stop Melee");
            meleeUI.SetActive(false);
            this.enabled = false;
            return;
        }

        Debug.Log("Stop Melee");
        isStabbing = false;
        simpleMeleeing = false;
        meleeCharging.charging = false;

        if (gun) {
            gun.transform.localPosition = originalGunLoc;

            if (!bm.batteryEquipped)
                gun.GetComponent<Gun>().autoReload();
        }

        playerMovement.canMove = true;
        enemyMovement.canMove = true;

        mouseLook.releaseMouse();
        resetMelee();

        if (killingBlow) {
            target.Die(true);
            killingBlow = false;
            meleeUI.SetActive(false);
            this.enabled = false;
        }
        killingBlow = false;
    }

    void resetMelee() {
        canStab = false;
        nextTimeToStab = Time.time + 1f / stabWait; //Sets the next time player can stab
    }

    //Any enemy damage event should go through here
    private void dealDamage(Target target, int damage) {
        hitmarker.enabled = true;

        if (target.health <= damage) {
            if (tellMM && mm) {
                Debug.Log("Melee Killed Target");
                mm.killedEnemy();
            }
            return; //Don't actually kill the target until melee is done
        }


        target.TakeDamage(damage);
    }

    public int getDamage() {
        return stabDamage;
    }

    #endregion
}