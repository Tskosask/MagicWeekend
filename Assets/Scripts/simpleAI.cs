using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class simpleAI : MonoBehaviour {
	public Transform player;
	public NavMeshAgent agent; 
	public Animator animator;

	public float beginChaseDistance; //Distance from player at which enemy is alerted and begins chase
	public float stopDistance; //Distance from the player at which to stop
	public float chaseSpeed; //Speed while chasing player
	public float rotationSpeed; //Speed of rotation 
	public float attackRange; //Distance from player at which enemy can attack
	public float positionSeekRadius; //Distance of random patrol sphere
	public float searchRefresh; //length of time before randomly picking new location
	
	private bool isAttacking = false;
	private float timeSinceLastSeek; //time elapsed since last randomPosition
	private float lastAttack;
	// Use this for initialization
	void Start () {
	//	Debug.Log("Player Position" + player.position);
	//	Debug.Log("agent Position" + this.transform.position);
		randomPosition();
	}
	
	// Update is called once per frame
	void Update () {
		checkDistance();
	}

	void randomPosition(){
		if(!canSeePlayer()){
			agent.destination = getRandomPosition();
		}
	}

	Vector3 getRandomPosition(){
		Vector3 random = (UnityEngine.Random.insideUnitSphere * positionSeekRadius) + this.transform.position;
		NavMeshHit hit;
		NavMesh.SamplePosition(random,out hit,positionSeekRadius,-1);
		return hit.position;
	}

	void checkDistance(){
		Vector3 dir = player.position - this.transform.position;
		if(canSeePlayer()){
			agent.destination = this.transform.position;
			dir.y = 0;
			this.transform.rotation = Quaternion.Slerp(this.transform.rotation,Quaternion.LookRotation(dir), rotationSpeed);	
			if(dir.magnitude > stopDistance){
				animator.SetBool("Moving", true);
				animator.SetBool("Running", true);
			}else{
				agent.destination = this.transform.position;
				animator.SetBool("Moving", false);
				animator.SetBool("Running", false);
			}

			//attack sequence
			if(dir.magnitude < attackRange && !isAttacking){
				isAttacking = true;
				Invoke("attack",Random.Range(1f,3f));
			}
		}else{
			timeSinceLastSeek += Time.deltaTime;
			if(timeSinceLastSeek >= searchRefresh){
				randomPosition();
				timeSinceLastSeek = 0;
			}
		}
	}

	bool canSeePlayer(){
		return (Vector3.Distance(player.position, this.transform.position) < beginChaseDistance);
	}

	void attack(){
		animator.SetTrigger("Attack1Trigger");
		StartCoroutine (pause(1.2f));
		isAttacking = false;
		//Debug.Log("Player Position" + player.position);
		//Debug.Log("agent Position" + this.transform.position);
		//Debug.Log("dir " + (player.position - this.transform.position));
	}
	
	public IEnumerator pause(float pauseTime){
		yield return new WaitForSeconds(pauseTime);
	}
}
