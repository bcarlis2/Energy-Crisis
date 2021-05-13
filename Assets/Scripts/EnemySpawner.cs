/*
	Project:    Energy Crisis
	
	Script:     EnemySpawner
	Desc:       Spawns a specified number of enemies within the bounds of the game object
	
	Credits:	Brandon Carlisle
	
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	#region Variables
	
	public GameObject enemyObject;
    public float xPos, zPos, xRange, zRange;
    //public int enemyCount;
    public int maxEnemies;
    bool running = false;

    public MissionManager mm;
    public bool tellMM = false;

    public int liveEnemies = 0;

    private bool infinite = false;

    #endregion

    #region Unity Methods

    public void Start()
    {
        StartCoroutine(EnemyDrop(maxEnemies));
    }

    public void Update()
    {
        if (!running)
            return;

        foreach(Transform child in transform) {
            if (child.childCount <= 0) {
                Destroy(child.gameObject);
            }
        }

        liveEnemies = transform.childCount;

        if (infinite && liveEnemies < maxEnemies) {
            spawn(maxEnemies - liveEnemies);
        }

        if (!infinite && liveEnemies <= 0) {
            Debug.Log("Spawner Wiped Out");

            if (tellMM && mm) {
                mm.wipedSpawner();
            }

            running = false;
        }
    }

    #endregion

    #region Methods

    public void spawn(int amount) {
        StartCoroutine(EnemyDrop(amount));
    }
	
	IEnumerator EnemyDrop(int amount, int enemyCount=0) {
        while (enemyCount < amount) {
            running = true;
            xRange = transform.localScale.x / 2;
            zRange = transform.localScale.z / 2;
            xPos = Random.Range((-1.0f) * xRange, xRange);
            zPos = Random.Range((-1.0f) * zRange, zRange);
            GameObject empty = new GameObject();
            empty.transform.parent = transform;
            empty.transform.localPosition = Vector3.zero;
            GameObject newBaddie = Instantiate(enemyObject, new Vector3(empty.transform.position.x + xPos, 3f, empty.transform.position.z + zPos), Quaternion.identity);
            //empty.transform.localRotation = Quaternion.identity;
            newBaddie.transform.parent = empty.transform; //Prevents crazy scaling problems
            //newBaddie.transform.localPosition = Vector3.zero;
            yield return new WaitForSeconds(0.1f);
            enemyCount++;
        }
    }

    public int getCount() {
        return transform.childCount;
    }

    public void spawnInfinite(int amount) {
        maxEnemies = amount;
        infinite = true;
        spawn(maxEnemies);
    }

    public void stopSpawnInfinite() {
        infinite = false;
        running = false;
    }

    #endregion
}