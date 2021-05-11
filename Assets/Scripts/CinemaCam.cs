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

public class CinemaCam : MonoBehaviour {

	#region Variables
	
	[SerializeField] Camera cineCam;
    [SerializeField] Canvas cineCanvas;
    [SerializeField] Transform anglesHolder;
    [SerializeField] GameObject credits;
    [SerializeField] Windmill windmill; //for the land, come together, hand in hand
    [SerializeField] Light oldLight;
    [SerializeField] Light[] newLights;
    [SerializeField] GameObject allFences;
    [SerializeField] Material newSkybox;
    PlayerMovement playerMovement;
    public bool returnToGame = false;
    public float shotLength = 5f;
    int angleNum = 0;

    #endregion

    #region Unity Methods

    public void Start()
    {
        startCinema();
        cineCam.enabled = true;
        cineCanvas.enabled = true;
        loopThrough();
    }

    public void Update()
    {
        //You're doing great!
    }

    #endregion

    #region Methods

    private void startCinema() {
        hidePlayer();
        oldLight.enabled = false;

        RenderSettings.skybox = newSkybox;

        foreach(Light newLight in newLights) {
            newLight.enabled = true;
        }

        windmill.enabled = true;
        allFences.SetActive(true);
    }

    private void hidePlayer() {
        playerMovement = PlayerMovement._instance;
        playerMovement.playerCanvas.SetActive(false);
        playerMovement.gameObject.SetActive(false);
    }

    private void unhidePlayer() {
        cineCanvas.enabled = false;
        cineCam.enabled = false;
        playerMovement.gameObject.SetActive(true);
        playerMovement.playerCanvas.SetActive(true);
    }

    private void loopThrough() {
        if (angleNum >= anglesHolder.childCount) {
            endCinema();
            return;
        }
        
        setAngle(anglesHolder.GetChild(angleNum));
        angleNum++;
        Invoke(nameof(loopThrough),shotLength);
    }
	
	private void setAngle(Transform angle) {
        cineCam.transform.position = angle.position;
        cineCam.transform.rotation = angle.rotation;
    }
    
    private void endCinema() {
        credits.SetActive(true);
    }

    public void endGame() {
        //We're in the endGame now
        playerMovement.gameObject.SetActive(true);
        playerMovement.toMenu();
    }

    #endregion
}