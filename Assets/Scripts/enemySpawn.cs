using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawn : MonoBehaviour {
	
	public float spawnRate;
	public int maxEnemies;
	public int numEnemiesAtStart;
	public GameObject enemy;

	private Transform[] spawnPoints;
	private float enemySpawnCounter;
	// Use this for initialization
	void Start () {
		//build spawnPoints list
		spawnPoints = this.GetComponentsInChildren<Transform>();

		//spawn the number of enemies to start with
		for(var i = 0; i < numEnemiesAtStart; i++){
			spawnEnemy();
		}

		//init counter
		enemySpawnCounter = 0;
	}
	
	// Update is called once per frame
	void Update () {
		enemySpawnCounter += Time.deltaTime;

		if(enemySpawnCounter >= spawnRate){
			enemySpawnCounter = 0;
			if(canSpawnEnemy()){
				spawnEnemy();
			}
		}
	}

	//get a random spawn point
	Vector3 getRandomSpawnLocation(){
		return spawnPoints[Random.Range(0,spawnPoints.Length)].position;
	}

	//spawn enemy at random spawn point
	void spawnEnemy(){
		//TODO: Remove unnecessary tmp var and debug after testing
		var tmp = getRandomSpawnLocation();
		//Debug.Log("SPAWNING AT: " + tmp);
		Instantiate(enemy,tmp,Quaternion.identity);
	}

	//determines if we can add anymore enemies to the game
	bool canSpawnEnemy(){
		return (GameObject.FindGameObjectsWithTag("enemy").Length < maxEnemies);
	}
}
