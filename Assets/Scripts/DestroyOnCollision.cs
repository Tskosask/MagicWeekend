using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCollision : MonoBehaviour {
    public GameObject replacementObject;

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
        GameObject replaced = Instantiate(replacementObject, transform.position, transform.rotation);
        Destroy(replaced, 3);
    }
}
