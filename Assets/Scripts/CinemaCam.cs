/*
	Project:    Energy Crisis
	
	Script:     CinemaCam
	Desc:       Handles the final cutscene of the game
	
	Credits:	Brandon Carlisle
	
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CinemaCam : MonoBehaviour {

	#region Variables
	
	[SerializeField] Camera cineCam;
    [SerializeField] Camera mainCam;
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
        AudioManager.instance?.toggleMusic(true,true);
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
        mainCam = playerMovement.GetComponentInChildren<Camera>();
        mainCam.gameObject.transform.SetParent(null); //Keep camera active for audio listener
        mainCam.enabled = false;
        mainCam.transform.GetChild(0).gameObject.SetActive(false); //Turn off guns so you can't shoot at end screen (though that's fun too)

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
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void endGame() {
        //We're in the endGame now
        playerMovement.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        //Time.timeScale = 1;
        SceneManager.LoadScene("TitleScreen");
        //playerMovement.toMenu();
    }

    #endregion
}