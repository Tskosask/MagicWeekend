using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class simpleAI : MonoBehaviour {
	
	public NavMeshAgent agent; 
	public Animator animator;
	public float beginChaseDistance; //Distance from player at which enemy is alerted and begins chase
	public float stopDistance; //Distance from the player at which to stop
	public float chaseSpeed; //Speed while chasing player
	public float rotationSpeed; //Speed of rotation 
	public float attackRange; //Distance from player at which enemy can attack
	public float positionSeekRadius; //Distance of random patrol sphere
	public float searchRefresh; //length of time before randomly picking new location
	public Material[] materials;
    public GameObject[] attacks;
    public GameObject FirePoint;

    private int enemyType;
    private GameObject enemyAttack;
	private Transform player;
	private bool isAttacking = false;
	private float timeSinceLastSeek; //time elapsed since last randomPosition
	private int attackCounter = 0; //increment attacks
	private int attackMove = 3; //at x attacks move position
	private Transform[] waypoints;

	// Use this for initialization
	void Start () {
		//set color of enemy randomly from list of select colors (potential weaknesses based on color)
		var renderer = this.GetComponentInChildren<Renderer>();
        enemyType = Random.Range(0, materials.Length);
        renderer.material = materials[enemyType];
        //get the attack of the enemy based on the material
        enemyAttack = attacks[enemyType];
		//always targe the player instance
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
		//add some variance to how often enemy moves while attacking
		attackMove = Random.Range(1,5); 

		//build waypoints list
		waypoints = GameObject.FindGameObjectWithTag("ground").GetComponentsInChildren<Transform>();

		//if a player is not in sight begin searching
		if(!canSeePlayer()){
			agent.destination = getRandomWaypoint();
		}
	}
	
	// Update is called once per frame
	void Update () {
		checkDistance();
	}

	Vector3 getRandomWaypoint(){
		return waypoints[Random.Range(0,waypoints.Length)].position;
	}

	//randomly move to a position, some radius (positionSeekRadius = default 10), from a given position
	Vector3 getRandomPosition(Vector3 position, float modifier = 1.0f){
		Vector3 random = (UnityEngine.Random.insideUnitSphere * (positionSeekRadius + modifier)) + position;
		NavMeshHit hit;
		NavMesh.SamplePosition(random,out hit,positionSeekRadius,-1);
		return hit.position;
	}

	//handles all distance related actions of the enemy unit
	void checkDistance(){
		//get difference between the player and this enemy unit
		Vector3 dir = player.position - this.transform.position;
		dir.y = 0;
		//set bools
		var inRangeCanAttack = (dir.magnitude < attackRange && !isAttacking);
		var shouldChangePos = (attackCounter >= attackMove);
		
		if(canSeePlayer()){
			if(shouldChangePos && inRangeCanAttack){
				attackCounter = 0; //reset counter
				agent.destination = getRandomPosition(player.position); //move enemy
			}else{
				//keep enemy facing player
				this.transform.rotation = Quaternion.Slerp(this.transform.rotation,Quaternion.LookRotation(dir), rotationSpeed);
				//animate based on distance from target
				animateMove(dir.magnitude > stopDistance);	

				if(inRangeCanAttack){
					isAttacking = true;
					Invoke("attack",Random.Range(1f,3f));
				}
			}
		}else{ //when player is not in alert distance 
			timeSinceLastSeek += Time.deltaTime; //concat frame completion times
			if(timeSinceLastSeek >= searchRefresh){ //when seeking exceeds max seek time
				//go to a new position and rest last seek counter
				agent.destination = getRandomWaypoint();
				timeSinceLastSeek = 0;
			}
		}
	}

	//animate move sequence
	void animateMove(bool isMoving){
		animator.SetBool("Moving", isMoving);
		animator.SetBool("Running", isMoving);
	}

	//returns if a player can be seen (is in range to start chase)
	bool canSeePlayer(){
		return (Vector3.Distance(player.position, this.transform.position) < beginChaseDistance);
	}

	//attack sequence
	void attack(){
		animator.SetTrigger("Attack1Trigger");
        Invoke("throwAttack", .5f);
		StartCoroutine (pause(1.2f));
		isAttacking = false;
		attackCounter++;
	}
	
    void throwAttack()
    {
        GameObject attackInstance = Instantiate(enemyAttack, new Vector3(FirePoint.transform.position.x, FirePoint.transform.position.y, FirePoint.transform.position.z), Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 90, transform.rotation.eulerAngles.z));
        attackInstance.tag = "enemyAttack";
        attackInstance.GetComponent<Rigidbody>().velocity = this.transform.forward * 3;
        Destroy(attackInstance, 10);
    }

    //pause for some duration
    public IEnumerator pause(float pauseTime){
		yield return new WaitForSeconds(pauseTime);
	}
}