using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalBattery : MonoBehaviour
{
    Battery battery;
    BatteryManager bm;
    BoxCollider realCollider;
    SphereCollider trigger;
    Rigidbody rgBody;
    Renderer appear;
    Outline outline;
    public bool isPhysical = true;
    int maxBatteries = 20;
    bool buffer = true;

    // Start is called before the first frame update
    void Start()
    {
        battery = GetComponent<Battery>();
        realCollider = GetComponent<BoxCollider>();
        trigger = GetComponent<SphereCollider>();
        rgBody = GetComponent<Rigidbody>();
        appear = GetComponent<Renderer>();

        //Adds outline at runtime
        if (gameObject.GetComponent<Outline>() == null) {
            outline = gameObject.AddComponent<Outline>();
            outline.OutlineMode = Outline.Mode.OutlineVisible;
            outline.OutlineColor = Color.green;
            outline.OutlineWidth = 10f;
        } else {
            outline = gameObject.GetComponent<Outline>();
        }

        Invoke(nameof(loadBuffer),1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Makes it so player can't reload on battery and grab it instantly
    void loadBuffer() {
        buffer = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !buffer) {
            Debug.Log("BATTERY COLLIDED");
            bm = other.gameObject.GetComponentInChildren<BatteryManager>();
            if (bm.numOfBatteries < maxBatteries) {
                transform.SetParent(bm.gameObject.transform); //Gets the Player's GameObject's child's BatteryManager's parent's transform
                //bm.refreshBatteryArray();
                bm.addBattery(battery);
                AudioManager.instance?.Play("Bloop");
                isPhysical = false;
                //Turn off everything that makes it physical
                trigger.enabled = false;
                realCollider.enabled = false;
                rgBody.isKinematic = false;
                rgBody.useGravity = false;
                appear.enabled = false;
                this.enabled = false;
            }

        }
    }
}
