/*
	Project:    Energy Crisis
	
	Script:     DoorAnimation
	Desc:       Swings open a door, used only in the Office
	
	Credits:	Brandon Carlisle
	
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimation : MonoBehaviour {

	#region Variables
	
	[SerializeField] Vector3 targetRotation;
    BoxCollider[] colliders;
    public int speed = 5;

    #endregion

    #region Unity Methods

    public void Start()
    {
        colliders = gameObject.GetComponents<BoxCollider>();
    }

    public void Update()
    {
        //Quaternion targetRotation = Quaternion.LookRotation(lookPos - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(targetRotation), speed * Time.deltaTime);

        if (transform.rotation == Quaternion.Euler(targetRotation)) {
            foreach(BoxCollider collider in colliders) {
                collider.enabled = false;
            }
            this.enabled = false;
        }
    }

    #endregion

    #region Methods
	
	

    #endregion
}