/*
	Project:    Energy Crisis
	
	Script:     AudioManager
	Desc:       Handles all sound effects and music, and is a singleton
	
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
    private AudioSource generator;

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
        Debug.Log("Sound: Played " + name);
        return s.source;
    }

    public void toggleMusic(bool toggle,bool ending=false) {
        Debug.Log("Music: " + toggle);
        if (toggle) {
            playMusic(ending);
        } else {
            stopMusic();
        }
    }

    public void playMusic(bool ending) {
        if (!enabledAudio)
            return;

        if (music)
            stopMusic();

        Debug.Log("Starting music");
        
        if (!ending) {
            music = Play("Beat");
            Debug.Log(music);
            return;
        }

        music = Play("EndMusic");
        Debug.Log(music);
        return;
    }

    public void stopMusic() {
        Debug.Log("Stopping Music");
        if (!music)
            return;

        music.Stop();
    }

    public void playGenerator() {
        generator = Play("Generator");
    }

    public void stopGenerator() {
        if (generator)
            generator.Stop();
    }

    #endregion
}