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
using UnityEngine.UI;
using TMPro;

public class PlayerMelee : MonoBehaviour {

	#region Variables
	
    [SerializeField] GameObject meleeUI;

    public Transform enemy;
    public Transform gun;
    public EnemyMovement enemyMovement;
	public Target target;

    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] MouseLook mouseLook;
    [SerializeField] MeleeCharging meleeCharging;

    bool isStabbing = false;
    public int stabDamage = 10;
    public int stabWait = 1;
    private float nextTimeToStab = 0f; //Has to start at zero
    public bool killingBlow = false;

    Vector3 originalGunLoc;

    #endregion

    #region Unity Methods

    public void Start()
    {
        //Hello :)
    }

    public void Awake() {

    }

    public void Update()
    {
        if (!enemyMovement.playerInMeleeRange) {
            disableMelee();
            return;
        }

        if (Input.GetButtonDown("Melee") && Time.time >= nextTimeToStab) {
            startMelee();
        }

        if (Input.GetButton("Melee") && isStabbing) {
            if (killingBlow) {
                keepMelee();
            } else {
                resetMelee();
            }
        }

        if (Input.GetButtonUp("Melee") && isStabbing) {
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
        
        playerMovement.canMove = false;
        enemyMovement.canMove = false;
        mouseLook.holdMouse();
        mouseLook.playerLookAt(new Vector3(enemy.position.x, transform.position.y, enemy.position.z));

        originalGunLoc = gun.localPosition;
        gun.position = transform.position + transform.forward * 2f; //Makes gun lunge forward

        if (target.health > stabDamage) { //Just take damage if they're not going to die
            target.TakeDamage(stabDamage);
        } else {
            Debug.Log("Starting Melee Charging...");
            killingBlow = true;
            meleeCharging.enabled = true;
        }
    }

    void keepMelee() {
        //Debug.Log("Keep Melee");
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
        meleeCharging.charging = false;

        gun.localPosition = originalGunLoc;

        playerMovement.canMove = true;
        enemyMovement.canMove = true;

        mouseLook.releaseMouse();

        if (killingBlow) {
            target.Die(true);
            meleeUI.SetActive(false);
            this.enabled = false;
        }
    }

    void resetMelee() {
        nextTimeToStab = Time.time + 1f / stabWait; //Sets the next time player can stab
    }

    #endregion
}