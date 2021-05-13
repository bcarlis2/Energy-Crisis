/*
	Project:    Energy Crisis
	
	Script:     Windmill
	Desc:       An infinite animation to have the windmill on the Farm rotate in the final cutscene
	
	Credits:	Brandon Carlisle
	
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Windmill : MonoBehaviour {

	#region Variables
	
	[SerializeField] Vector3 targetRotation;
    public Quaternion startRotation;
    public int speed = 5;
    public float i = 0;

    #endregion

    #region Unity Methods

    public void Start()
    {
    }

    public void Update()
    {
        if (i >= 360)
            i = 0;
        
        //Quaternion targetRotation = Quaternion.LookRotation(lookPos - transform.position);
        transform.localRotation = Quaternion.Euler(0,0,i);

        i += speed * Time.deltaTime;
    }

    #endregion

    #region Methods
	
	

    #endregion
}