using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {

    public float speed = 5f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float lowRange = 1f;
        float highRange = 2f;

        //rotate randomly every frame?
        transform.Rotate(0, 0, Random.Range(lowRange, highRange));
	}
}
