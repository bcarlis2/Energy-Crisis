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

public class EnemySpawner : MonoBehaviour {

	#region Variables
	
	public GameObject enemyObject;
    public float xPos, zPos, xRange, zRange;
    //public int enemyCount;
    public int maxEnemies;

    #endregion

    #region Unity Methods

    public void Start()
    {
        StartCoroutine(EnemyDrop(maxEnemies));
    }

    public void Update()
    {
        //You're doing great!
    }

    #endregion

    #region Methods

    public void spawn(int amount) {
        StartCoroutine(EnemyDrop(amount));
    }
	
	IEnumerator EnemyDrop(int amount, int enemyCount=0) {
        while (enemyCount < amount) {
            xRange = transform.localScale.x / 2;
            zRange = transform.localScale.z / 2;
            xPos = Random.Range((-1.0f) * xRange, xRange);
            zPos = Random.Range((-1.0f) * zRange, zRange);
            Instantiate(enemyObject, new Vector3(transform.position.x + xPos, 3f, transform.position.z + zPos), Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
            enemyCount++;
        }
    }

    #endregion
}