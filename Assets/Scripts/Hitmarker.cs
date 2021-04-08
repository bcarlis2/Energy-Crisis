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

public class Hitmarker : MonoBehaviour {

	#region Variables
	
	Image hitIcon;

    #endregion

    #region Unity Methods

    public void Start()
    {
    }

    public void Update()
    {
        //You're doing great!
    }

    public void OnEnable() {
        hitIcon = GetComponent<Image>();
        hitIcon.enabled = true;
        Invoke(nameof(end),0.15f);
    }

    #endregion

    #region Methods
	
	public void end() {
        hitIcon.enabled = false;
        this.enabled = false;
    }

    #endregion
}