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

public class Door : MonoBehaviour {

	#region Variables
	
	

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
	
	    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) {
            Debug.Log("OPEN DOOR");

            SceneManager.LoadScene("Alleyway");

        }
    }

    #endregion
}