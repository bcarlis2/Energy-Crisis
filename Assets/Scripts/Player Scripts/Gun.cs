using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Add reload
public class Gun : MonoBehaviour
{
    [Space(10)]
    [Header("Object References")]
    [Tooltip("Use for the Raycast")]
    public Camera fpsCam;
    [Tooltip("Partical effect for muzzle flash")]
    public ParticleSystem muzzleFlash;
    [Tooltip("Partical effect for bullet impact")]
    public GameObject impactEffect;

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
    Battery battery;

    void Start()
    {
        battery = bm.getBattery(); //Battery Manager will find the battery for the gun
    }


    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire && battery.charge >= chargeCost){
            nextTimeToFire = Time.time + 1f / fireRate; //Sets the next time the gun will be ready to fire
            Shoot();
        }
    }

    void Shoot() {
        RaycastHit hit;

        battery.use(chargeCost);

        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range )) { //Raycasts forward to the given range
            Target target = hit.transform.GetComponent<Target>(); //Gets the information of the target hit
            if (target != null) {
                target.TakeDamage(damage); //Damage the target if there is one
            }

            if (hit.rigidbody != null) {
                hit.rigidbody.AddForce(-hit.normal * impactForce); //If whatever was hit has a rigidbody, push it
            }
        }
    }
}
