/*
	Project:	
	
	Script:		
	Desc:		
	
	Last Edit:	
	Credits:	Brandon Carlisle
	
*/

using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	#region Variables
	
	public Sound[] sounds;

    public static AudioManager instance;

    public bool enabledAudio = true;

    private AudioSource music;

    #endregion

    #region Unity Methods

    void Awake() {

        if (instance != null && instance != this) {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);

        foreach(Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    void Start() {
        toggleMusic(enabledAudio);
    }

    #endregion

    #region Methods
	
	public AudioSource Play(string name) {
        if (!enabledAudio)
            return null;

        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null) {
            Debug.LogWarning("Sound: " + name + "not found!");
            return null;
        }

        s.source.Play();
        return s.source;
    }

    public void toggleMusic(bool toggle) {
        Debug.Log("Music: " + toggle);
        if (toggle) {
            playMusic();
        } else {
            stopMusic();
        }
    }

    public void playMusic() {
        music = Play("Beat");
        Debug.Log(music);
    }

    public void stopMusic() {
        if (!music)
            return;

        music.Stop();
    }

    #endregion
}