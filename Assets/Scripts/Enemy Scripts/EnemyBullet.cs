/*
	Project:    Energy Crisis
	
	Script:     EnemyBullet
	Desc:       Handles damaging the player as well as despawning the bullets
	
	Credits:	Brandon Carlisle
	
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour {

	#region Variables
	
	PlayerHealth playerHealth;
    public GameObject thisBullet;
    int secondsTilDespawn = 5;
    int damage = 5;

    #endregion

    #region Unity Methods

    public void Start()
    {
        Invoke(nameof(delete),secondsTilDespawn);
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

            Debug.Log("You've been shot!");
            playerHealth = other.gameObject.GetComponentInChildren<PlayerHealth>();
            playerHealth.takeDamage(damage);
            delete();
        }
    }

    private void delete() {
        Destroy(thisBullet);
    }

    #endregion
}