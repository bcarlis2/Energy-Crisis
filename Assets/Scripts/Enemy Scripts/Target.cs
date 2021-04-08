using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Target : MonoBehaviour
{
    [Space(10)]
    [Header("References")]
    public Transform me;
    public Transform player;
    public GameObject chargingField;
    public GameObject deadModel;
    public EnemyMovement enemyMovement;
    public NavMeshAgent nma;

    [Space(10)]
    [Header("Attributes")]
    public float maxHealth = 100;
    public float triggerDistance = 10f;

    //[HideInInspector]
    public float health;
    bool dead = false;
    bool meleeOutline = false;
    int playerMeleeDamage;


    void Start()
    {
        if (!player) {
            player = GameObject.Find("Player").transform;
        }

        playerMeleeDamage = player.gameObject.GetComponent<PlayerMelee>().getDamage();

        health = maxHealth;
        enemyMovement = GetComponent<EnemyMovement>();
        chargingField = transform.GetChild(0).gameObject;
        deadModel = transform.GetChild(1).gameObject;
    }


    void Update()
    {
        /* EnemyMovement handles this now


        float dist = Vector3.Distance(me.position,player.position);

        if (Mathf.Abs(dist) <= triggerDistance) {
            Vector3 lookPos = new Vector3(player.position.x, me.position.y, player.position.z);
            transform.LookAt(lookPos);
        }


        */

        if (health <= playerMeleeDamage && !meleeOutline) {
            //Adds outline at runtime
            var outline = gameObject.AddComponent<Outline>();
            outline.OutlineMode = Outline.Mode.OutlineVisible;
            outline.OutlineColor = Color.red;
            outline.OutlineWidth = 20f;
            meleeOutline = true;
        }
    }

    public void TakeDamage(float amount) {
        health -= amount;

        if (health <= 0f && !dead) {
            Die();
        }
    }

    public void Die(bool skipCharge = false) {
        dead = true;
        enemyMovement.Dying();
        enemyMovement.enabled = false;

        Renderer[] bodyParts = GetComponentsInChildren<Renderer>();

        foreach (Renderer bodyPart in GetComponentsInChildren<Renderer>()) {
            bodyPart.enabled = false;
        }

        deadModel.SetActive(true);

        if (!skipCharge) {  //Activates charging field and detatches it. Else, it destroys with this
            chargingField.SetActive(true);
            chargingField.transform.SetParent(null);
        }

        //GetComponent<Renderer>().enabled = false;
        //Destroy(gameObject);

        deadModel.transform.SetParent(null); //Detatches dead model

        Destroy(gameObject); //Destroys this and everything left attached to this
    }
}
