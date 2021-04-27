using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Space(10)]
    [Header("Object References")]
    [Tooltip("Use for the Raycast")]
    public Camera fpsCam;
    //[Tooltip("Partical effect for muzzle flash")]
    //public ParticleSystem muzzleFlash;
    [Tooltip("Used to check pause")]
    public MouseLook mouseLook;
    [Tooltip("Slide of gun")]
    public Transform gunSlide;
    [Tooltip("Partical effect for bullet impact")]
    public GameObject impactEffect;
    [Tooltip("Spotlight used for muzzle flash")]
    public Light muzzleFlash;
    [Tooltip("Hitmaker")]
    public Hitmarker hitmarker;

    public enum GunType { Pistol, Shotgun };
    [SerializeField] public GunType gunType;
    public BatteryManager.Type batteryType;

    [Space(10)]
    [Header("Gun Attributes")]
    public float damage = 10f;
    public float range = 100f;
    public int chargeCost = 5;
    [Tooltip("In fractions of a second, 1/X")]
    public float fireRate = 1f;
    public float impactForce = 30f;

    private float nextTimeToFire = 0f; //Has to start at zero

    public BatteryManager bm;
    public Battery battery;
    Battery oldBattery;

    private Outline outline;

    [SerializeField] MissionManager mm;
    public bool tellMM;

    void Start()
    {
        switch (gunType) {
            case GunType.Pistol:
                batteryType = BatteryManager.Type.AAA;
                break;
            case GunType.Shotgun:
                batteryType = BatteryManager.Type.D;
                break;
            default:
                batteryType = BatteryManager.Type.AAA;
                break;
        }

        battery = bm.getBattery(batteryType,chargeCost); //Battery Manager will find the battery for the gun
        outline = gameObject.GetComponent<Outline>();
        outline.enabled = false;
        hitmarker = transform.parent.parent.parent.GetComponentInChildren<Hitmarker>(); //Looks sketchy
    }

    void OnEnable() {
        battery = bm.getBattery(batteryType,chargeCost); //Battery Manager will find the battery for the gun
    }

    void OnDisable() {
        removeBattery();
    }


    void Update()
    {
        battery?.changeState(BatteryManager.State.InUse); //Just in case it gets changed, may not be needed?

        if (mouseLook.paused)
            return;

        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire) {
            if (battery?.charge >= chargeCost) {
                nextTimeToFire = Time.time + 1f / fireRate; //Sets the next time the gun will be ready to fire
                Shoot();
            } else {
                AudioManager.instance.Play("Empty");
            }
        }

        if (Input.GetButtonDown("Reload")) {
            Reload();
        }

        if (Input.GetButtonDown("Unload")) {
            removeBattery();
        }
    }

    void Shoot() {
        RaycastHit hit;

        muzzleFlash.enabled = true;

        if (gunType == GunType.Pistol) {
            gunSlide.localPosition = new Vector3(gunSlide.localPosition.x, gunSlide.localPosition.y, gunSlide.localPosition.z +0.02f);
            AudioManager.instance.Play("PistolFire");
        } else if (gunType == GunType.Shotgun) {
            gunSlide.localPosition = new Vector3(gunSlide.localPosition.x, gunSlide.localPosition.y, gunSlide.localPosition.z -0.1f);
            AudioManager.instance.Play("ShotgunFire");
        }
        Invoke(nameof(turnOffMuzzleFlash),0.2f);


        battery.use(chargeCost);
        //Debug.Log("SHOOTING " + battery.toString());

        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range )) { //Raycasts forward to the given range
            Target target = hit.transform.GetComponent<Target>(); //Gets the information of the target hit
            if (target != null) {
                dealDamage(target,damage); //Damage the target if there is one
            }

            if (hit.rigidbody != null) {
                hit.rigidbody.AddForce(-hit.normal * impactForce); //If whatever was hit has a rigidbody, push it
            }
        }

        if (tellMM && mm) {
            mm.gotFire();
        }
    }

    void Reload() {
        //Debug.Log("Reloading!");
        oldBattery = battery;
        battery = bm.getBattery(batteryType,chargeCost,battery); //Battery Manager will find a battery for the gun

        if (battery && oldBattery && battery != oldBattery) {
            //Debug.Log("SWAPPING " + oldBattery.toString() + " FOR " + battery.toString());
            oldBattery.changeState(BatteryManager.State.Inventory);
            //nextTimeToFire = 0f; //No cooldown if you reload?
        }

        if (tellMM && mm) {
            mm.gotReload();
        }
    }

    //If needed to seperate battery from gun
    public void removeBattery() {
        battery = null;
        //bm.unload();
    }

    //Turns off the muzzle flash after a short amount of time
    public void turnOffMuzzleFlash() {

        if (gunType == GunType.Pistol) {
            gunSlide.localPosition = new Vector3(gunSlide.localPosition.x, gunSlide.localPosition.y, gunSlide.localPosition.z -0.02f);
        } else if (gunType == GunType.Shotgun) {
            gunSlide.localPosition = new Vector3(gunSlide.localPosition.x, gunSlide.localPosition.y, gunSlide.localPosition.z +0.1f);
        }

        muzzleFlash.enabled = false;
    }

    //Any enemy damage event should go through here
    private void dealDamage(Target target, float damage) {
        hitmarker.enabled = true;
        target.TakeDamage(damage);
    }
}
