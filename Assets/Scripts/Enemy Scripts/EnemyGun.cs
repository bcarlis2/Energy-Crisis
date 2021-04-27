/*
	Project:	Energy Crisis
	
	Script:		Enemy Gun
	Desc:		Aims at and shoots the player
	
	Last Edit:	
	Credits:	Brandon Carlisle
	
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : MonoBehaviour {

	#region Variables
	
	Vector3 aimLoc;
    [SerializeField] GameObject muzzleFlash;
    public float bulletForce = 32f;
    public float shootOffset = 1f;
    public float bulletRange = 50f;
    public int bulletDamage = 5;
    public float impactForce = 10f;
    public float maxBuffer = 1f;

    #endregion

    #region Unity Methods

    public void Start()
    {
        //Hello :)
    }

    public void Update()
    {
        //You're doing great!
    }

    #endregion

    #region Methods

    public void shoot(Vector3 aim) {
        aimLoc = aim;
        float buffer = generateBuffer();
        if (buffer <= 0.0f) {
            Debug.Log("INSTANT FIRE");
            fire();
        } else {
            Invoke(nameof(fire),buffer); //Slight variation, some instant hits and some near misses
        }
    }

    public float generateBuffer() {
        return Random.Range(-0.1f,maxBuffer);
    }
	
	private void fire() {
        /* RAYCAST ROUTE */
        RaycastHit hit;
        transform.LookAt(aimLoc);

        muzzleFlash.SetActive(true);
        AudioManager.instance.Play("EnemyFire");
        Invoke(nameof(turnOffMuzzleFlash),0.2f);

        if (Physics.Raycast(transform.position, transform.forward, out hit, bulletRange )) { //Raycasts forward to the given range
            //Debug.Log("Enemy Shot Something");
            PlayerHealth target = hit.transform.GetComponent<PlayerHealth>(); //Gets the information of the target hit
            target?.takeDamage(bulletDamage); //Damage the target if there is one

            if (hit.rigidbody != null) {
                hit.rigidbody.AddForce(-hit.normal * impactForce); //If whatever was hit has a rigidbody, push it
            }
        }
    }
    
    private void turnOffMuzzleFlash() {
        muzzleFlash.SetActive(false);
    }

    #endregion
}