/*
	Project:    Energy Crisis
	
	Script:     Player Health
	Desc:       Handles player's health
	
	Credits:	Brandon Carlisle
	
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour {

	#region Variables
	
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] TextMeshProUGUI healthGUI;
    PlayerMelee playerMelee;

	int health = 100;
    int maxHealth = 100;
    int prevHealth = 0;
    bool dead = false;

    #endregion

    #region Unity Methods

    public void Start()
    {
        health = maxHealth;
        playerMelee = GetComponent<PlayerMelee>();
    }

    public void Update()
    {
        if (health != prevHealth) { //Trying to not update the text every frame if nothing changes
            healthGUI.SetText("Health: " + health + "/" + maxHealth);
        }
        prevHealth = health;
    }

    #endregion

    #region Methods
	
	public void takeDamage(int damage) {

        if (playerMelee.isStabbing)
            return;

        if (!dead) {
            int tempNewHealth = health - damage;

            if (tempNewHealth > 0) {    //Simple damage
                health = tempNewHealth;
            } else {                    //Death
                die();
            }
        }
    }

    private void die() {
        dead = true;
        health = 0;

        playerMovement.canMove = false;
        //playerMovement.enabled = false; //Is it a bad idea to disable the whole script?

        Invoke(nameof(restart), 5); //Restarts scene 5 seconds after death, only called once!
    }

    private void restart() {
        SceneManager.LoadScene("SampleScene");
    }

    #endregion
}