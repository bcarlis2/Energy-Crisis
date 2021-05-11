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
    [SerializeField] MissionManager mm;
    public bool tellMM;

    [Space(10)]
    [Header("Attributes")]
    public float maxHealth = 100;
    public float triggerDistance = 10f;

    //[HideInInspector]
    public float health;
    bool dead = false;
    public bool meleeOutline = false;
    int playerMeleeDamage;


    void Start()
    {
        if (!player) {
            player = GameObject.Find("Player").transform;
        }

        PlayerMelee pm = player.gameObject.GetComponent<PlayerMelee>();

        playerMeleeDamage = pm.getDamage();
        mm = player.gameObject.GetComponent<MissionManager>();

        if (health <= 0)
            health = maxHealth;
            
        enemyMovement = GetComponent<EnemyMovement>();
        chargingField = transform.GetChild(0).gameObject;
        chargingField.GetComponent<ChargingField>().setFilter(pm.meleeCharging.blueFilter);
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

    public void Die(bool skipCharge = false) { //Will count as player kill (maybe)

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

        if (tellMM && mm) {
            Debug.Log("Target telling MM that it killed it");
            mm.killedEnemy();
            Invoke(nameof(destroyBuffer),0.5f); //Maybe that'll force the method to run
        } else {
            Destroy(gameObject); //Destroys this and everything left attached to this
        }
    }

    private void destroyBuffer() {
        Destroy(gameObject);
    }

    void OnDestroy() {
        if (!dead) 
            return; //Hopefully the "dead" variable only gets activated if player kills it

        if (tellMM && mm) {
            Debug.Log("Final Destroy");
            //mm.killedEnemy();
        }
    }
}
