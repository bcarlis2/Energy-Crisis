using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingField : MonoBehaviour
{
    BatteryManager bm;
    Battery battery;
    Battery oldBattery;
    [SerializeField] float chargeAmount = 100f;
    [SerializeField] float seconds = 5;
    float buffer = 0;
    [SerializeField] float intervals = 0.5f;
    public bool charging = false; //Player is in field
    float give = 0f;
    [SerializeField] public GameObject blueFilter;
    private PlayerHealth playerHealth;
    private AudioSource audio;

    //Debugging
    public float totalCharge = 0;

    void Start()
    {
    }

    void Update()
    {

        //if (playerHealth != null)
        //       playerHealth.invinsible = charging;

        buffer -= Time.deltaTime;
        if (blueFilter)
            blueFilter.SetActive(charging); //Might get overwritten by other charging fields

        if (buffer <= 0) {
            give = chargeAmount / (seconds / intervals);
            totalCharge += give;
            //Debug.Log(chargeAmount + "/" + seconds + "=" + give);
            chargeAmount -= give;

            //battery?.changeState(BatteryManager.State.Inventory); //Resets the state in case it's not charging again

            if (charging) { //Player is in field
                
                if (battery) {
                    oldBattery = battery;
                } else {
                    oldBattery = null;
                }

                battery = bm.chargeBattery(give); //Tells the Battery Manager to find a battery and charge it, and returns the battery reference

                if (battery != oldBattery) {
                    Debug.Log("Charging Field: Not the same battery");
                    oldBattery?.changeBackState();
                }
                battery?.changeState(BatteryManager.State.Charging);
            }

            seconds -= intervals;
            buffer = intervals;
        }

        if (seconds <= 0) {
            battery?.changeBackState(); //Resets the state of the last battery to charge
            if (blueFilter)
                blueFilter.SetActive(false);

            if (playerHealth != null)
                playerHealth.invinsible = false;
            
            //if (audio)
            //    audio.Stop();
            
            //Destroy(transform.parent.gameObject); //Destroys the enemy parent
            Destroy(this.gameObject); //Destroys self
        }

        //Debug.Log("HI:"); //Lol I don't know why I put this here

        /*
        if (timeRemaining > 0) {
            give = chargeAmount / timeRemaining;
            chargeAmount -= give;
            if (charging) {
                bm.chargeBattery(give);
                Debug.Log("CF: charging battery");
            }
        }
            */
        

        /*
            TIME REMAINING = 10:
                give = 100 / 10 = 10
                chargeAmount = 90
            TIME REAMINING = 9:
                give = 90 / 9 = 10
                chargeAmount = 80
        */
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) {

            if (bm == null) {
                bm = other.gameObject.GetComponentInChildren<BatteryManager>();
            }

            if (blueFilter == null) {
                blueFilter = bm.blueFilter;
            }

            if (playerHealth == null) {
                playerHealth = other.gameObject.GetComponentInChildren<PlayerHealth>();
            }

            playerHealth.invinsible = true;
            blueFilter.SetActive(true);
            charging = true;
            audio = AudioManager.instance?.Play("Charging");

            //bm.chargeBattery();
            //Debug.Log("Charging Started");
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player")) {

            playerHealth.invinsible = false;
            blueFilter.SetActive(false);
            charging = false;

            if (audio)
                audio.Stop();

            battery?.changeBackState(); //Resets the state of the last battery to charge
            //Debug.Log("Charging Stopped");
        }
    }

    public void setFilter(GameObject filter) {
        blueFilter = filter;
    }
}
