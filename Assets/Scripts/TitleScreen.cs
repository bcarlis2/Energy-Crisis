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
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour {

	#region Variables
	
	

    #endregion

    #region Unity Methods

    public void Start()
    {
        //Hello :)
        GameObject player = GameObject.Find("Player");

        if (player)
            Destroy(player);
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

    public void exitGame() {
        Application.Quit();
    }

    #endregion
}