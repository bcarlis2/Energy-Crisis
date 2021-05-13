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
    Color[] healthColors = new Color[4];

	int health = 100;
    int maxHealth = 100;
    int prevHealth = 0;
    bool dead = false;
    public bool invinsible = false;
    public float iframesTime = 0.5f;
    bool iframes = false;

    #endregion

    #region Unity Methods

    public void Start()
    {
        health = maxHealth;
        playerMelee = GetComponent<PlayerMelee>();

        healthColors[0] = Color.white;
        healthColors[1] = new Color(1,0.6f,0.2f); //Orange
        healthColors[2] = Color.red;
        healthColors[3] = Color.green;
    }

    public void Update()
    {
        if (health != prevHealth) { //Trying to not update the text every frame if nothing changes
            healthGUI.SetText(health + "");
        }
        prevHealth = health;
    }

    #endregion

    #region Methods

    public void setHealth(int inHealth) {
        health = inHealth;
    }

    public int getHealth() {
        return health;
    }

    public bool isFull() {
        return health == maxHealth;
    }

    public void giveHealth(int amount) {
        if ((health + amount) > maxHealth) {
            setHealth(maxHealth);
            //Simple color animation
            healthGUI.color = healthColors[3];
            Invoke(nameof(setHealthColor),0.5f);
            return;
        }
        setHealth(health + amount);
        //Simple color animation
        healthGUI.color = healthColors[3];
        Invoke(nameof(setHealthColor),0.5f);
    }
	
	public void takeDamage(int damage) {

        if (iframes || invinsible || playerMelee.isStabbing)
            return;

        if (!dead) {
            int tempNewHealth = health - damage;
            AudioManager.instance?.Play("Hurt");

            //Simple color animation
            healthGUI.color = healthColors[1];
            Invoke(nameof(setHealthColor),0.5f);

            if (tempNewHealth > 0) {    //Simple damage
                health = tempNewHealth;
                iframes = true;
                Invoke(nameof(restIFrames),iframesTime);
            } else {                    //Death
                die();
            }
        }
    }

    private void setHealthColor() {
        if (health > 25) {
            healthGUI.color = healthColors[0];
        } else {
            healthGUI.color = healthColors[2];
        }
    }

    private void restIFrames() {
        iframes = false;
    }

    private void die() {
        dead = true;
        health = 0;

        playerMovement.canMove = false;
        //playerMovement.enabled = false; //Is it a bad idea to disable the whole script?
        GetComponentInChildren<MissionManager>().setText("You Died");

        Invoke(nameof(restart), 5); //Restarts scene 5 seconds after death, only called once!
    }

    private void restart() {
        //SaveData.instance?.Load();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene("TitleScreen");
    }

    #endregion
}