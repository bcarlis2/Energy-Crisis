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
    public float pulseWait = 1f;
    public bool enemiesInRange = false;
    [SerializeField] public Material glow;
    [SerializeField] public Material oMat;

    //LineRenderer lr;
    private Renderer renderer;
    public SphereCollider sphereC;

    #endregion

    #region Unity Methods

    public void Start() {
        sphereC = gameObject.GetComponent<SphereCollider>();
        renderer = GetComponent<Renderer>();
        //lr = gameObject.GetComponent<LineRenderer>();

    }

    public void Update() {

    }

    #endregion

    #region Methods

    public void startShocking() {
        sphereC.enabled = true;
        pulseLoop();
    }

    private void pulseLoop() {
        sendPulse();
        Invoke(nameof(pulseLoop),pulseWait);
    }

    public void sendPulse() {
        if (targets.Count == 0)
            return;
    
        int rand = Random.Range(0,targets.Count);
        Debug.Log("Shooting at Target " + rand);
        Target doomedOne = targets[rand];

        if (doomedOne == null)
            return;

        targets.Remove(doomedOne);
        //lr.SetPosition(0, doomedOne.gameObject.transform.position);
        doomedOne.TakeDamage(50);
        renderer.material = glow;
        Invoke(nameof(noGlow),pulseWait/2);
    }

    public void noGlow() {
        renderer.material = oMat;
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