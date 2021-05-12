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

public class PowerPole : MonoBehaviour {

	#region Variables
	
    //[SerializeField] public BatteryManager.Type batteryType;
    public List<Target> targets;
    public LayerMask whatIsEnemy;
    public float sightRange = 50f;
    public float pulseWait = 2f;
    public float totalTime = 30f;
    public bool enemiesInRange = false;
    [SerializeField] public Material glow;
    [SerializeField] public Material oMat;

    //LineRenderer lr;
    [SerializeField] public Renderer renderer1, renderer2;
    public SphereCollider sphereC;
    public Interactable interactable;

    private bool stop = false;

    #endregion

    #region Unity Methods

    public void Start() {
        sphereC = gameObject.GetComponent<SphereCollider>();
        interactable = gameObject.GetComponent<Interactable>();
        //renderer = GetComponent<Renderer>();
        //lr = gameObject.GetComponent<LineRenderer>();

    }

    public void Update() {

    }

    #endregion

    #region Methods

    public void startShocking() {
        interactable.enabled = false;
        sphereC.enabled = true;
        stop = false;
        pulseLoop();
        Invoke(nameof(endShocking),totalTime);
    }

    public void endShocking() {
        Debug.Log("Stop Shocking");
        stop = true;
        sphereC.enabled = false;
        this.enabled = false;
    }

    private void pulseLoop() {
        sendPulse();
        Invoke(nameof(pulseLoop),pulseWait);
    }

    public void sendPulse() {
        if (targets.Count == 0 || stop)
            return;
    
        int rand = Random.Range(0,targets.Count);
        Debug.Log("Shooting at Target " + rand + "out of " + targets.Count);
        Target doomedOne = targets[rand];

        if (doomedOne == null)
            return;

        targets.Remove(doomedOne);
        //lr.SetPosition(0, doomedOne.gameObject.transform.position);
        doomedOne.getShocked(75);
        renderer1.material = glow;
        renderer2.material = glow;
        AudioManager.instance?.Play("PowerPole");
        Invoke(nameof(noGlow),pulseWait/2);
    }

    public void noGlow() {
        renderer1.material = oMat;
        renderer2.material = oMat;
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Enemy")) {
            Debug.Log("PP found Enemy");
            targets.Add(other.gameObject.GetComponent<Target>());
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Enemy")) {
            targets.Remove(other.gameObject.GetComponent<Target>());
        }
    }
	
	

    #endregion
}