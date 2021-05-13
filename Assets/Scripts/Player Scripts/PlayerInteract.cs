/*
	Project:    Energy Crisis
	
	Script:     PlayerInteract
	Desc:       Blueprint for more specific scripts dealing with player pressing E to interact with objects
	
	Credits:	Brandon Carlisle
	
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInteract : MonoBehaviour {

	#region Variables
	
	[SerializeField] Canvas interactCanvas;
    [SerializeField] TextMeshProUGUI interactText;
    Interactable interactable;
    bool guiOn = false;

    #endregion

    #region Unity Methods

    public void Start()
    {
        interactCanvas.enabled = false;
    }

    public void Update()
    {
        if (!guiOn)
            return;
        
        if (Input.GetButtonDown("Interact")) {
            interactable.interact();
            closeInteractGUI(interactable);
        }
    }

    #endregion

    #region Methods
	
	public void openInteractGUI(Interactable inter) {
        interactable = inter;
        interactText.SetText(interactable.text);
        interactCanvas.enabled = true;
        guiOn = true;
    }

    public void closeInteractGUI(Interactable inter) {
        if (inter != interactable)
            return;
        
        interactable = null;
        interactCanvas.enabled = false;
        guiOn = false;
    }

    #endregion
}