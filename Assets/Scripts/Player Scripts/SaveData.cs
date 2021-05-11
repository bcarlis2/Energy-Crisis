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

public class SaveData : MonoBehaviour {

	#region Variables
	
	public int health;
    public ArrayList batteries;
    public ArrayList guns;

    [SerializeField] public PlayerHealth playerHealth;
    public BatteryManager bm;
    public Transform bmTrans;
    public WeaponSwitcher ws;
    public Transform wsTrans;

    public static SaveData instance;

    //Just for debugging
    [SerializeField] public int batteriesLength;
    [SerializeField] public int weaponsLength;
    [SerializeField] public Transform saveStorage;
    public int childCounter = 0;
    bool loading = false;


    #endregion

    #region Unity Methods

    void Awake() {

        if (instance != null && instance != this) {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
        //clearAll();
    }

    public void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        bm = GetComponentInChildren<BatteryManager>();
        bmTrans = bm.gameObject.transform;
        ws = GetComponentInChildren<WeaponSwitcher>();
        wsTrans = ws.gameObject.transform;

        health = 100;
        batteries = new ArrayList();
        guns = new ArrayList();

        if (saveStorage == null)
            saveStorage = transform;
    }



    public void Update()
    {
        //Just for debugging
        batteriesLength = batteries.Count;
        weaponsLength = guns.Count;

        childCounter = (bmTrans.childCount + wsTrans.childCount);

        //if (loading && childCounter <= 0) {
        //    loading = false;
        //    loadSaved();
        //}
    }

    #endregion

    #region Methods
	
	public void Save() {
        Debug.Log(" !!! SAVING !!!");
        health = playerHealth.getHealth();

        //Remove whatever batteries and guns are in save storage
        foreach (Transform child in saveStorage) {
            child.SetParent(null);
            Destroy(child.gameObject);
        }
        batteries.Clear();
        guns.Clear();


        foreach (Transform child in bmTrans) {
            GameObject saveBattery = Instantiate(child.gameObject,saveStorage);
            batteries.Add(saveBattery.transform);
        }


        foreach(Transform child in wsTrans) {
            GameObject saveGun = Instantiate(child.gameObject,saveStorage);
            saveGun.SetActive(false);
            guns.Add(saveGun.transform);
        }
    }

    public void clearAll() {
        //Debug.Log(" !!! CLEARING !!!");
        //if (!bm)
        //    return;

        //Debug.Log(" !!! CLEARING !!!");

        //bm.clearBatteryAray();
        //ws.clearWeapons();

        /*
        //Remove whatever batteries and guns are on the player
        foreach (Transform child in bmTrans) {
            child.SetParent(null);
            child.gameObject.SetActive(false);
            Destroy(child.gameObject);
        }

        foreach (Transform child in wsTrans) {
            child.SetParent(null);
            child.gameObject.SetActive(false);
            Destroy(child.gameObject);
        }
        */
    }

    public void Load() {

        /*
        foreach (Transform battery in batteries) {
            Instantiate(battery.gameObject,bmTrans);
        }
        bm.refreshBatteryArray();
        Debug.Log(bmTrans.childCount);

        foreach (Transform gun in guns) {
            Instantiate(gun.gameObject,wsTrans);
        }
        ws.equipNew();
        */

        Debug.Log("Loading");

        foreach (Transform child in saveStorage) {
            if (child.gameObject.GetComponent<Battery>()) {
                Instantiate(child.gameObject,bmTrans);
            } else if (child.gameObject.GetComponent<Gun>()) {
                Instantiate(child.gameObject,wsTrans);
            }
        }


        Debug.Log("Refreshing");
        bm.refreshBatteryArray();
        ws.equipNew();


    }

    #endregion
}