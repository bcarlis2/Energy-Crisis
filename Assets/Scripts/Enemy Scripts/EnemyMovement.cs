using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public NavMeshAgent agent;
    public Target target;

    public GameObject playerObj;
    public Transform player;
    public PlayerMelee playerMelee;
    private int playerMeleeDamage;
    public bool canMeleeDie;

    public LayerMask whatIsGround, whatIsPlayer;

    //Patroling
    public Vector3 walkPoint;
    public bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    float timeBetweenAttacksMin = 1f;
    float timeBetweenAttacksMax = 5f;
    bool alreadyAttacked;
    public bool canMove;
    public bool canAttack;
    public GameObject projectile;
    public EnemyGun gun;

    //States
    public float sightRange, attackRange, stopRange, meleeRange;
    public bool playerInSightRange, playerInAttackRange, playerInStopRange, playerInMeleeRange, meleeable, dying;


    private void Awake() {
        playerObj = GameObject.Find("Player");
        player = playerObj.transform;
        playerMelee = playerObj.GetComponent<PlayerMelee>();

        target = GetComponent<Target>();
        agent = GetComponent<NavMeshAgent>();
        gun = GetComponentInChildren<EnemyGun>();

        canMove = true;
        canAttack = true;
        canMeleeDie = false;
    }

    private void Update() {
        //Check for sight and attack range
        if (dying) {
            Dying();
            return;
        }
  
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        playerInStopRange = Physics.CheckSphere(transform.position, stopRange, whatIsPlayer);
        playerInMeleeRange = Physics.CheckSphere(transform.position, meleeRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange && !playerInStopRange) Patroling();
        if (playerInSightRange && !playerInAttackRange && !playerInStopRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer(playerInStopRange);

        if (playerInMeleeRange && !meleeable) tellPlayerMelee();
        if (!playerInMeleeRange && meleeable) stopPlayerMelee();
    }

    private void Patroling() {

        if (!canMove) {
            agent.SetDestination(transform.position); //Stops enemy
            return;
        }


        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet) agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 2f)
            walkPointSet = false;
    }

    private void SearchWalkPoint() {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer() {

        if (!canMove) {
            agent.SetDestination(transform.position); //Stops enemy
            return;
        }

        agent.SetDestination(player.position);
    }

    private void AttackPlayer(bool stop) {

        if (!canMove) {
            agent.SetDestination(transform.position); //Stops enemy
            return;
        }
        
        //Make sure enemy doesn't move if player is in stop range
        Vector3 lookPos = new Vector3(player.position.x, transform.position.y, player.position.z);

        if (stop) {
            agent.SetDestination(transform.position);
            transform.LookAt(lookPos);
        } else {
            agent.SetDestination(player.position);
        }

        if (!alreadyAttacked && canAttack) {

            /* PHYSICS ROUTE
            //Projectile
            Vector3 bulletPos = new Vector3(transform.position.x, transform.position.y - shootOffset, transform.position.z);
            GameObject bullet = Instantiate(projectile, bulletPos, Quaternion.identity);
            bullet.GetComponent<EnemyBullet>().thisBullet = bullet; //This is kinda nuts
            bullet.transform.LookAt(lookPos);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * bulletForce, ForceMode.Impulse);
            //rb.AddForce(transform.up * 32f, ForceMode.Impulse);
            */

            gun.shoot(player.position);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), Random.Range(timeBetweenAttacksMin, timeBetweenAttacksMax));
        }
    }

    private void ResetAttack() {
        alreadyAttacked = false;
    }

    public void Dying() {
        agent.isStopped = true;
        stopPlayerMelee();
    }

    private void tellPlayerMelee() {
        if (playerMelee == null)
            return;

        meleeable = true;
        Debug.Log("Telling Player Melee");
        playerMelee.enabled = true; //Should it turn off if nothing to melee?
        playerMelee.setTarget(this,target,transform);
    }

    private void stopPlayerMelee() {
        if (playerMelee == null)
            return;

        meleeable = false;
        Debug.Log("Enemy stopped player melee script");
        playerMelee.stopMelee();
    }

    /*
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
    */
}
