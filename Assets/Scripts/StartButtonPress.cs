using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButtonPress : MonoBehaviour {

    private float buttonTravel = 0.02f;

    private void OnTriggerEnter(Collider other)
    {
        //push button down
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - buttonTravel);
        this.GetComponent<AudioSource>().Play();
        SceneManager.LoadScene("Death Valley");
    }

    private void OnTriggerExit(Collider other)
    {
        //let button back up
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + buttonTravel);
        this.GetComponent<AudioSource>().Play();

    }
}
