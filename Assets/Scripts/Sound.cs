/*
	Project:	
	
	Script:		
	Desc:		
	
	Last Edit:	
	Credits:	Brandon Carlisle
	
*/

using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound {

	#region Variables
	
    public string name;
	public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;

    [Range(0.1f, 3f)]
    public float pitch;

    public bool loop;

    [HideInInspector]
    public AudioSource source;

    #endregion


    #region Methods
	
	

    #endregion
}