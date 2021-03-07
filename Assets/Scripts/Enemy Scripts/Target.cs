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


    void Start()
    {
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

        if (skipCharge)
            return;

        deadModel.SetActive(true);
        //GetComponent<Renderer>().enabled = false;
        chargingField.SetActive(true);
        //Destroy(gameObject);
    }
}
