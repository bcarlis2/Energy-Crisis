/*
	Project:    Energy Crisis
	
	Script:     Title Screen
	Desc:       Handles button interaction on the main menu
	
	Credits:	Brandon Carlisle
	
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour {

	#region Variables
	
	[SerializeField] public GameObject howToPlayGO;
    [SerializeField] public GameObject creditsGO;
    public bool howToPlay = false;
    public bool credits = false;

    #endregion

    #region Unity Methods

    public void Start()
    {
        //Hello :)
        GameObject player = GameObject.Find("Player");

        if (player)
            Destroy(player);

        GameObject wrongCamera = GameObject.Find("Main Camera");

        if (wrongCamera)
            Destroy(wrongCamera);

        AudioManager.instance.stopMusic();
        AudioManager.instance.playMusic(false); //Stop whatever music is playing and restart main theme

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Update()
    {
        //You're doing great!
    }

    #endregion

    #region Methods
	
	public void startGame() {
        SceneManager.LoadScene("Warehouse");
    }

    public void toggleFullscreen() {
        Screen.fullScreen = !Screen.fullScreen;
    }

    public void toggleAudio() {
        if (!AudioManager.instance)
            return;
        
        AudioManager.instance.toggleMusic(!AudioManager.instance.enabled);
        AudioManager.instance.enabled = !AudioManager.instance.enabled;
    }

    public void toggleHowToPlay() {
        howToPlay = !howToPlay;
        credits = false;
        howToPlayGO.SetActive(howToPlay);
        creditsGO.SetActive(false);
    }

    public void toggleCredits() {
        credits = !credits;
        howToPlay = false;
        creditsGO.SetActive(credits);
        howToPlayGO.SetActive(false);
    }

    public void exitGame() {
        Application.Quit();
    }

    #endregion
}