using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [Space(10)]
    [Header("Refeerences")]
    public Transform me;
    public Transform player;

    [Space(10)]
    [Header("Attributes")]
    public float maxHealth = 100;
    public float triggerDistance = 10f;

    //[HideInInspector]
    public float health;


    void Start()
    {
        health = maxHealth;
    }


    void Update()
    {
        float dist = Vector3.Distance(me.position,player.position);

        if (Mathf.Abs(dist) <= triggerDistance) {
            Vector3 lookPos = new Vector3(player.position.x, me.position.y, player.position.z);
            transform.LookAt(lookPos);
        }
    }

    public void TakeDamage(float amount) {
        health -= amount;

        if (health <= 0f) {
            Die();
        }
    }

    void Die() {
        Destroy(gameObject);
    }
}
